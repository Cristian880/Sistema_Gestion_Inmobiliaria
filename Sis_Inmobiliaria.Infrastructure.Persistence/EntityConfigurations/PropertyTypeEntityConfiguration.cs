using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sis_Inmobiliaria.Core.Domain.Entities;

namespace Sis_Inmobiliaria.Infrastructure.Persistence.EntityConfigurations
{
    internal class PropertyTypeEntityConfiguration
    {
        public void Configure(EntityTypeBuilder<PropertyType> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("PropertyType");
            #endregion

            #region Property configurations
            builder.Property(u => u.Name).IsRequired().HasMaxLength(200);
            #endregion

            #region relationships
            #endregion
        }
    }
}
