using Services;
using Services.Abstractions;

namespace E_Commerce.API.Extensions
{
    public static class CoreServicesExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {

            services.AddScoped<IServicesManger, ServicesManger>();
            services.AddAutoMapper(typeof(Services.AssemblyReference).Assembly);
            return services;
        }
    }
}
