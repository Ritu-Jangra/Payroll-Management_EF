namespace Payroll.API.CustomConfigurations
{
    using Asp.Versioning.ApiExplorer;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider versionDescriptionProvider)
        {
            _provider = versionDescriptionProvider;
        }

        public void Configure(string? name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        /// <summary>
        /// Configures SwaggerGenOptions to generate a Swagger document for each API version defined in the application.
        /// </summary>
        /// <param name="options">Options for Swagger generation</param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var items in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(items.GroupName, CreateVersionInfo(items));
            }
        }

        /// <summary>
        /// Creates an OpenApiInfo object for a given API version description, which includes the title and version information for the Swagger document.
        /// </summary>
        /// <param name="description">Description of the API version</param>
        /// <returns>Returns the OpenApiInfo object</returns>
        private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = $"Payroll API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
            };

            return info;
        }
    }
}
