using Microsoft.Extensions.Options;
using ServiceStack.AspNetCore.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ServiceStack;

public class ConfigureServiceStackSwagger :
    IConfigureOptions<SwaggerGenOptions>,
    IConfigureOptions<ServiceStackOptions>
{
    private readonly OpenApiMetadata metadata;

    public ConfigureServiceStackSwagger(OpenApiMetadata metadata)
    {
        this.metadata = metadata;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var filterType in metadata.DocumentFilterTypes)
        {
            options.DocumentFilterDescriptors.Add(new FilterDescriptor
            {
                Type = filterType,
                Arguments = [metadata],
            });
        }
        foreach (var filterType in metadata.SchemaFilterTypes)
        {
            options.SchemaFilterDescriptors.Add(new FilterDescriptor
            {
                Type = filterType,
                Arguments = [],
            });
        }
    }

    public void Configure(ServiceStackOptions options)
    {
        options.WithOpenApi();
    }
}
