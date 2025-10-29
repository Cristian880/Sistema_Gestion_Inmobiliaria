using Microsoft.Extensions.DependencyInjection;
using Sis_Inmobiliaria.Core.Application.Interfaces;
using Sis_Inmobiliaria.Core.Application.Services;
using System.Reflection;


namespace Sis_Inmobiliaria.Core.Application.ViewModels.User
{
    public static class ServicesRegistration
    {
        //Extension method - Decorator pattern
        public static void AddApplicationLayerIoc(this IServiceCollection services)
        {
            #region Configurations
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            #endregion
            #region Services IOC
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<IPropertyTypeService, PropertyTypeService>();
            // aqui se utiliza AddScoped porque tambien se maneja por instancias del usuario
            #endregion
        }

    }
}
