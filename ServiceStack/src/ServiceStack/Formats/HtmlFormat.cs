using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ServiceStack.Serialization;
using ServiceStack.Web;

namespace ServiceStack.Formats
{
    public class HtmlFormat : IPlugin, Model.IHasStringId
    {
        public string Id { get; set; } = Plugins.Html;
        public static string TitleFormat
            = @"{0} Snapshot of {1}";

        public static string HtmlTitleFormat
            = @"Snapshot of <i>{0}</i> generated by <a href=""https://servicestack.net"">ServiceStack</a> on <b>{1}</b>";

        public static bool Humanize = true;

        private IAppHost AppHost { get; set; }
        
        public Dictionary<string, string> PathTemplates { get; set; } = new() {
            { "/" + LocalizedStrings.Auth.Localize(), "/Templates/auth.html" }
        };
        
        public Func<IRequest, string> ResolveTemplate { get; set; }

        public string DefaultResolveTemplate(IRequest req)
        {
            if (PathTemplates != null && PathTemplates.TryGetValue(req.PathInfo, out var templatePath))
            {
                var file = HostContext.VirtualFileSources.GetFile(templatePath);
                if (file == null)
                    throw new FileNotFoundException($"Could not load HTML template '{templatePath}'", templatePath);

                return file.ReadAllText();
            }
            return null;
        }

        public HtmlFormat()
        {
            ResolveTemplate = DefaultResolveTemplate;
        }

        public void Register(IAppHost appHost)
        {
            AppHost = appHost;
            //Register this in ServiceStack with the custom formats
            appHost.ContentTypes.RegisterAsync(MimeTypes.Html, SerializeToStreamAsync, null);
            appHost.ContentTypes.RegisterAsync(MimeTypes.JsonReport, SerializeToStreamAsync, null);

            appHost.Config.DefaultContentType = MimeTypes.Html;
            appHost.Config.IgnoreFormatsInMetadata.Add(MimeTypes.Html.ToContentFormat());
            appHost.Config.IgnoreFormatsInMetadata.Add(MimeTypes.JsonReport.ToContentFormat());
        }

        public async Task SerializeToStreamAsync(IRequest req, object response, Stream outputStream)
        {
            var res = req.Response;
            if (req.GetItem("HttpResult") is IHttpResult httpResult && httpResult.Headers.ContainsKey(HttpHeaders.Location) 
                && httpResult.StatusCode != System.Net.HttpStatusCode.Created)  
                return;

            try
            {
                if (res.StatusCode >= 400)
                {
                    var responseStatus = response.GetResponseStatus();
                    req.Items[Keywords.ErrorStatus] = responseStatus;
                }

                if (response is CompressedResult)
                {
                    if (res.Dto != null)
                        response = res.Dto;
                    else 
                        throw new ArgumentException("Cannot use Cached Result as ViewModel");
                }

                foreach (var viewEngine in AppHost.ViewEngines)
                {
                    var handled = await viewEngine.ProcessRequestAsync(req, response, outputStream);
                    if (handled)
                        return;
                }
            }
            catch (Exception ex)
            {
                if (res.StatusCode < 400)
                    throw;

                //If there was an exception trying to render a Error with a View, 
                //It can't handle errors so just write it out here.
                response = DtoUtils.CreateErrorResponse(req.Dto, ex);
            }

            //Handle Exceptions returning string
            if (req.ResponseContentType == MimeTypes.PlainText)
            {
                req.ResponseContentType = MimeTypes.Html;
                res.ContentType = MimeTypes.Html;
            }

            if (req.ResponseContentType != MimeTypes.Html && req.ResponseContentType != MimeTypes.JsonReport) 
                return;

            var dto = response.GetDto();
            if (!(dto is string html))
            {
                // Serialize then escape any potential script tags to avoid XSS when displaying as HTML
                var json = JsonDataContractSerializer.Instance.SerializeToString(dto) ?? "null";
                json = json.HtmlEncodeLite();

                var url = req.ResolveAbsoluteUrl()
                    .Replace("format=html", "")
                    .Replace("format=shtm", "")
                    .TrimEnd('?', '&')
                    .HtmlEncode();

                url += url.Contains("?") ? "&" : "?";

                var now = DateTime.UtcNow;
                var requestName = req.OperationName ?? dto.GetType().GetOperationName();

                html = ReplaceTokens(ResolveTemplate?.Invoke(req) ?? Templates.HtmlTemplates.GetHtmlFormatTemplate(), req)
                    .Replace("${RequestName}", requestName)
                    .Replace("${RequestDto}", JsonDataContractSerializer.Instance.SerializeToString(req.Dto)?.HtmlEncodeLite() ?? "null")
                    .Replace("${Dto}", json)
                    .Replace("${Title}", string.Format(TitleFormat, requestName, now))
                    .Replace("${MvcIncludes}", MiniProfiler.Profiler.RenderIncludes().ToString())
                    .Replace("${Header}", string.Format(HtmlTitleFormat, requestName, now))
                    .Replace("${ServiceUrl}", url)
                    .Replace("${Humanize}", Humanize.ToString().ToLower())
                    ;
            }
            
            await ((ServiceStackHost)AppHost).WriteAutoHtmlResponseAsync(req, response, html, outputStream);
        }

        public static string ReplaceTokens(string html, IRequest req)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            html = html
                .Replace("${BaseUrl}", req.GetBaseUrl().TrimEnd('/'))
                .Replace("${AuthRedirect}", req.ResolveAbsoluteUrl(HostContext.AppHost.GetPlugin<AuthFeature>()?.HtmlRedirect))
                .Replace("${AllowOrigins}", HostContext.AppHost.GetPlugin<CorsFeature>()?.AllowOriginWhitelist?.Join(";"))
            ;
            return html;
        }
    }
}