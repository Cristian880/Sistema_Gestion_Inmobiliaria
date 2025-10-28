using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sis_Inmobiliaria.Core.Domain.Interfaces;
using Sis_Inmobiliaria.Infrastructure.Persistence.Contexts;
using Sis_Inmobiliaria.Infrastructure.Persistence.Repositories;

namespace ClassLibrary4
{
    public static class ServicesRegistration
    {
        //Extension method - Decorator pattern
        public static void AddPersistenceLayerIoc(this IServiceCollection services, IConfiguration config)
        {
            #region Contexts
            if (config.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<Sis_InmobiliariaAppContext>(opt =>
                                              opt.UseInMemoryDatabase("AppDb"));
            }
            else
            {
                var connectionString = config.GetConnectionString("DefaultConnection");
                services.AddDbContext<Sis_InmobiliariaAppContext>(
                  (serviceProvider, opt) =>
                  {
                      opt.EnableSensitiveDataLogging();
                      opt.UseSqlServer(connectionString,
                      m => m.MigrationsAssembly(typeof(Sis_InmobiliariaAppContext).Assembly.FullName));
                  },
                    contextLifetime: ServiceLifetime.Scoped,// se puso scoped porque el DbContext solo por usuario tal vez maneje 5-6 transacciones en la db, pero si se crea otro usuario, se crea otro DbContext

                    //Antes se usaba Transient, pero ahora se usa Scoped porque el DbContext es un servicio que debe ser creado una vez por solicitud HTTP y no por cada instancia de un servicio. ademas de que no se necesita que sea un servicio de larga duración.Osea que no se necesita algo que meneje una entrada a la base de datos por cada transacción, sino que se maneje una por usuario y no por cada instancia de un servicio.
                    optionsLifetime: ServiceLifetime.Scoped
                 );

                #endregion

                #region Repositories IOC
                services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
                services.AddScoped<IPropertyRepository, PropertyRepository>();
                services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
                
                #endregion
            }
        }
    }
}
