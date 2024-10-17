using E_Commerce.API.Factories;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Services;
using Microsoft.OpenApi.Models;
namespace E_Commerce.API.Extensions
{
    public static class PresentationServicesExtensions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {

            services.AddControllers().AddApplicationPart(typeof(AssemblyReference).Assembly);
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationResponse;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.ConfigureSwagger();
            return services;


        }
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please Enter Bearer Token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "Jwt"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference= new OpenApiReference
                        {
                            Type= ReferenceType.SecurityScheme,
                            Id="Bearer",

                        }
                    },
                    new List<string>(){}
                } });
            }


            );
            return services;
        }
    }
}
