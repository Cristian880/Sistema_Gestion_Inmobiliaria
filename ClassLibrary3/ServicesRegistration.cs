using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sis_Inmobiliaria.Core.Application.Interfaces;
using Sis_Inmobiliaria.Infrastructure.Identity.Contexts;
using Sis_Inmobiliaria.Infrastructure.Identity.Entities;
using Sis_Inmobiliaria.Infrastructure.Identity.Seeds;
using Sis_Inmobiliaria.Infrastructure.Identity.Services;

namespace Sis_Inmobiliaria.Infrastructure.Identity
{
    public static class ServicesRegistration
    {
        public static void AddIdentityLayerIocForWebApp(this IServiceCollection services, IConfiguration config)
        {
            GeneralConfiguration(services, config);

            #region Identity 
            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Password.RequireNonAlphanumeric = true;//caracteres especiales
                opt.Password.RequireLowercase = true;// requiere letras minúsculas
                opt.Password.RequireUppercase = true;// requiere letras mayúsculas

                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);//se desbloquea automaticamente despues de 5 minutos
                opt.Lockout.MaxFailedAccessAttempts = 5;

                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;// Requiere que el email esté confirmado para iniciar sesión
            });

            // Agrega servicios de Identity usando la entidad AppUser y roles
            services.AddIdentityCore<AppUser>() // Usuario personalizado
                .AddRoles<IdentityRole>() // Soporte para roles
                .AddSignInManager() // Servicio para manejar login
                .AddEntityFrameworkStores<IdentityContext>() // Usa IdentityContext como base de datos
                .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider); // Para generar tokens (confirmación, recuperación), reseteo de contraseña, etc.

            // Configura duración del token (por ejemplo, para confirmación de email)
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(12);// tiempo de vida del token de protección de datos
            });

            // Configura el esquema de autenticación con cookies
            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = IdentityConstants.ApplicationScheme;// Esquema por defecto para autenticación con cookies
                opt.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
            }).AddCookie(IdentityConstants.ApplicationScheme, opt =>
            {
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(180);// Sesión expira en 3 horas
                opt.LoginPath = "/Login"; // Ruta de login
                opt.AccessDeniedPath = "/Login/AccessDenied"; // Ruta si no tiene permisos



                // con opt.SlidingExpiration = true; // Habilita expiración deslizante (renueva sesión si está activa)
            });
            #endregion

            #region Services
            // Registra un servicio personalizado que implementa lógica de autenticación/autorización
            services.AddScoped<IAccountServiceForWebApp, AccountServiceForWebApp>();
            #endregion
        }
        // Método de extensión para sembrar (crear) datos de Identity al arrancar la app
        public static async Task RunIdentitySeedAsync(this IServiceProvider service)
        {
            using var scope = service.CreateScope();// Crea un scope manual
            var servicesProvider = scope.ServiceProvider;

            // Obtiene los servicios necesarios para crear usuarios y roles
            var userManager =
                servicesProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = servicesProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Crea los roles por defecto
            await DefaultRoles.SeedAsync(roleManager);
            await DefaultUser.SeedAsync(userManager);// Crea un usuario tipo "Inversor"
            await DefaultAdminUser.SeedAsync(userManager);// Crea un usuario administrador

        }

        #region Private methods
        // Método privado que configura el DbContext de Identity
        private static void GeneralConfiguration(IServiceCollection services, IConfiguration config)
        {
            #region Contexts
            if (config.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(opt =>
                                              opt.UseInMemoryDatabase("AppDb"));
            }
            else
            {
                var connectionString = config.GetConnectionString("DefaultConnection");
                services.AddDbContext<IdentityContext>(

                    (serviceProvider, opt) =>
                    {
                        opt.EnableSensitiveDataLogging(); // Habilita logging detallado (para debugging)
                        opt.UseSqlServer(connectionString,
                        m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));// Apunta a las migraciones correctas
                    },
                    contextLifetime: ServiceLifetime.Scoped, // Tiempo de vida para el DbContext
                    optionsLifetime: ServiceLifetime.Scoped
                );
            }
            #endregion
        }

        #endregion
    }
}
