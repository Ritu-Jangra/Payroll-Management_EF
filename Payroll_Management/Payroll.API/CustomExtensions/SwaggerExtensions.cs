namespace Payroll.API
{
    using Microsoft.OpenApi.Models;

    public static class SwaggerExtensions
    {
        /// <summary>
        /// Adds Swagger configuration to the service collection, including JWT authentication support.
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Returns the updated service collection</returns>
        public static IServiceCollection AddSwaggerWithJwtAuthentication(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                // Define the Bearer authentication scheme
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token like: Bearer {your token}"
                });

                // Add a global security requirement for the Bearer scheme
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            return services;
        }
    }
}
