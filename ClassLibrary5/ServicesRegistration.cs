using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sis_Inmobiliaria.Core.Application.Interfaces;
using Sis_Inmobiliaria.Core.Domain.Settings;
using Sis_Inmobiliaria.Infrastructure.Shared.Services;

namespace Sis_Inmobiliaria.Infrastructure.Shared
{
    public static class ServicesRegistration
    {
        //Extension method - Decorator pattern
        public static void AddSharedLayerIoc(this IServiceCollection services, IConfiguration config)
        {
            #region Configurations
            services.Configure<MailSettings>(config.GetSection("MailSettings"));
            #endregion

            #region Services IOC
            services.AddScoped<IEmailService, EmailService>();
            #endregion
        }
    }
}
