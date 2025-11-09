using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Sis_Inmobiliaria.Infrastructure.Shared
{
    // Provides no-op extension methods referenced from Program.cs so the project compiles
    // and keeps the fluent call chain. Implement logic here if you need to register
    // application-wide static properties for endpoints or pages.
    public static class StaticPropertysExtensions
    {
        public static WebApplication MapStaticPropertys(this WebApplication app)
        {
            // Example: load commonly used data and store in app.Properties or in-memory cache
            // var propertyTypes = app.Services.GetRequiredService<IPropertyTypeService>().GetAllWithInclude();
            // app.Properties["PropertyTypes"] = propertyTypes;

            return app;
        }

        public static T WithStaticPropertys<T>(this T builder) where T : IEndpointConventionBuilder
        {
            // This is intentionally a no-op placeholder so calls like
            // app.MapRazorPages().WithStaticPropertys(); compile and continue to work.
            return builder;
        }
    }
}
