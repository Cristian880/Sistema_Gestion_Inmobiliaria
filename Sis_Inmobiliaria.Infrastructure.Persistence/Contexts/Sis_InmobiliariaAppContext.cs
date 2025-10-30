using Microsoft.EntityFrameworkCore;
using Sis_Inmobiliaria.Core.Domain.Entities;
using System.Reflection;


namespace Sis_Inmobiliaria.Infrastructure.Persistence.Contexts
{
    public class Sis_InmobiliariaAppContext : DbContext
    {
        public Sis_InmobiliariaAppContext(DbContextOptions<Sis_InmobiliariaAppContext> options) : base(options) { }
        public DbSet<Property> Property { get; set; }
        public DbSet<PropertyType> PropertyType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //Liskov-substitution

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
