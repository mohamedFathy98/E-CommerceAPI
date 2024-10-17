using Domain.Contracts;

namespace E_Commerce.API.Extensions
{
    public static class WebApplicationExtensions
    {


        public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
        {
            //Create object from type implemnts IDbInitializer
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializerAsync();
            await dbInitializer.InitializerIdentityAsync();
            return app;
        }
    }
}
