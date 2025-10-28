using Microsoft.EntityFrameworkCore;
using Sis_Inmobiliaria.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Inmobiliaria.Infrastructure.Persistence.Contexts
{
    public class InvestmentAppContext : DbContext
    {
        public InvestmentAppContext(DbContextOptions<InvestmentAppContext> options) : base(options) { }
        public DbSet<PropertyStatus> PropertyStatus { get; set; }
        public DbSet<PropertyType> PropertyType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //Liskov-substitution

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
