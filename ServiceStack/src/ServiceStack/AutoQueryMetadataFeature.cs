using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;
using ServiceStack.NativeTypes;

namespace ServiceStack;

public class AutoQueryMetadataFeature : IPlugin, Model.IHasStringId
{
    public string Id { get; set; } = Plugins.AutoQueryMetadata;
    public AutoQueryViewerConfig AutoQueryViewerConfig { get; set; }
    public Action<AutoQueryMetadataResponse> MetadataFilter { get; set; }
    public List<Type> ExportTypes { get; set; } 
    public int? MaxLimit { get; set; }

    public AutoQueryMetadataFeature()
    {
        this.AutoQueryViewerConfig = GetAutoQueryViewerConfigDefaults();
        this.ExportTypes = [typeof(RequestLogEntry)];
    }

    internal static AutoQueryViewerConfig GetAutoQueryViewerConfigDefaults()
    {
        return new AutoQueryViewerConfig
        {
            Formats = ["json", "xml", "csv"],
            ImplicitConventions = [
                new AutoQueryConvention { Name = "=", Value = "%" },
                new AutoQueryConvention { Name = "!=", Value = "%!" },
                new AutoQueryConvention { Name = ">=", Value = ">%" },
                new AutoQueryConvention { Name = ">", Value = "%>" },
                new AutoQueryConvention { Name = "<=", Value = "%<" },
                new AutoQueryConvention { Name = "<", Value = "<%" },
                new AutoQueryConvention { Name = "In", Value = "%In" },
                new AutoQueryConvention { Name = "Between", Value = "%Between" },
                new AutoQueryConvention { Name = "Starts With", Value = "%StartsWith", Types = "string" },
                new AutoQueryConvention { Name = "Contains", Value = "%Contains", Types = "string" },
                new AutoQueryConvention { Name = "Ends With", Value = "%EndsWith", Types = "string" },
            ]
        };
    }

    public void Register(IAppHost appHost)
    {
        if (MaxLimit != null)
            AutoQueryViewerConfig.MaxLimit = MaxLimit;

        appHost.RegisterService<AutoQueryMetadataService>();
    }
}

public class AutoQueryViewerConfig : AppInfo
{
    /// <summary>
    /// The BaseUrl of the ServiceStack instance (inferred)
    /// </summary>
    public string ServiceBaseUrl { get; set; }
    /// <summary>
    /// The different Content Type formats to display
    /// </summary>
    public string[] Formats { get; set; }
    /// <summary>
    /// The configured MaxLimit for AutoQuery
    /// </summary>
    public int? MaxLimit { get; set; }
    /// <summary>
    /// Whether to publish this Service to the public Services registry
    /// </summary>
    public bool IsPublic { get; set; }
    /// <summary>
    /// Only show AutoQuery Services attributed with [AutoQueryViewer]
    /// </summary>
    public bool OnlyShowAnnotatedServices { get; set; }
    /// <summary>
    /// List of different Search Filters available
    /// </summary>
    public List<AutoQueryConvention> ImplicitConventions { get; set; }

    /// <summary>
    /// The Column which should be selected by default
    /// </summary>
    public string DefaultSearchField { get; set; }
    /// <summary>
    /// The Query Type filter which should be selected by default
    /// </summary>
    public string DefaultSearchType { get; set; }
    /// <summary>
    /// The search text which should be populated by default
    /// </summary>
    public string DefaultSearchText { get; set; }
}

[ExcludeMetadata]
[Route("/autoquery/metadata")]
public class AutoQueryMetadata : IReturn<AutoQueryMetadataResponse> { }

public class AutoQueryViewerUserInfo : IMeta
{
    /// <summary>
    /// Returns true if the User Is Authenticated
    /// </summary>
    public bool IsAuthenticated { get; set; }

    /// <summary>
    /// How many queries are available to this user
    /// </summary>
    public int QueryCount { get; set; }

    public Dictionary<string, string> Meta { get; set; }
}

public class AutoQueryOperation : IMeta
{
    public string Request { get; set; }
    public List<MetadataRoute> Routes { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public Dictionary<string, string> Meta { get; set; }
}

public class AutoQueryMetadataResponse : IMeta
{
    public AutoQueryViewerConfig Config { get; set; }

    public AutoQueryViewerUserInfo UserInfo { get; set; }

    public List<AutoQueryOperation> Operations { get; set; }

    public List<MetadataType> Types { get; set; }

    public ResponseStatus ResponseStatus { get; set; }

    public Dictionary<string, string> Meta { get; set; }
}

[Restrict(VisibilityTo = RequestAttributes.None)]
public class AutoQueryMetadataService(INativeTypesMetadata metadata) : Service
{
    public async Task<object> AnyAsync(AutoQueryMetadata request)
    {
        if (metadata == null)
            throw new NotSupportedException("AutoQueryViewer requires NativeTypesFeature");

        var feature = HostContext.GetPlugin<AutoQueryMetadataFeature>();
        var config = feature.AutoQueryViewerConfig;

        if (config == null)
            throw new NotSupportedException("AutoQueryViewerConfig is missing");

        config.ServiceBaseUrl ??= base.Request.GetBaseUrl();
        config.ServiceName ??= HostContext.ServiceName;

        var userSession = await Request.GetSessionAsync();

        var typesConfig = metadata.GetConfig(new TypesMetadata { BaseUrl = Request.GetBaseUrl() });
        foreach (var type in feature.ExportTypes)
        {
            typesConfig.ExportTypes.Add(type);
        }

        var metadataTypes = metadata.GetMetadataTypes(Request, typesConfig, 
            op => HostContext.Metadata.IsAuthorized(op, Request, userSession));

        var response = new AutoQueryMetadataResponse {
            Config = config,
            UserInfo = new AutoQueryViewerUserInfo {
                IsAuthenticated = userSession.IsAuthenticated,
            },
            Operations = [],
            Types = [],
        };

        var includeTypeNames = new HashSet<string>();

        foreach (var op in metadataTypes.Operations)
        {
            if (op.Request.Inherits != null 
                && (op.Request.Inherits.Name.StartsWith("QueryDb`") || 
                    op.Request.Inherits.Name.StartsWith("QueryData`"))
               )
            {
                if (config.OnlyShowAnnotatedServices)
                {
                    var serviceAttrs = op.Request.Attributes.Safe();
                    var attr = serviceAttrs.FirstOrDefault(x => x.Name + "Attribute" == nameof(AutoQueryViewerAttribute));
                    if (attr == null)
                        continue;
                }

                var inheritArgs = op.Request.Inherits.GenericArgs.Safe().ToArray();
                response.Operations.Add(new AutoQueryOperation {
                    Request = op.Request.Name,
                    Routes = op.Routes,
                    From = inheritArgs.First(),
                    To = inheritArgs.Last(),
                });

                response.Types.Add(op.Request);
                op.Request.GetReferencedTypeNames(metadataTypes).Each(x => includeTypeNames.Add(x));
            }
        }

        var allTypes = metadataTypes.GetAllTypes().ToList();
        var types = allTypes.Where(x => includeTypeNames.Contains(x.Name)).ToList();

        //Add referenced types to type name search
        types.SelectMany(x => x.GetReferencedTypeNames(metadataTypes)).Each(x => includeTypeNames.Add(x));

        //Only need to seek 1-level deep in AutoQuery's (db.LoadSelect)
        types = allTypes.Where(x => includeTypeNames.Contains(x.Name)).ToList();

        response.Types.AddRange(types);

        response.UserInfo.QueryCount = response.Operations.Count;

        feature.MetadataFilter?.Invoke(response);

        return response;
    }
}