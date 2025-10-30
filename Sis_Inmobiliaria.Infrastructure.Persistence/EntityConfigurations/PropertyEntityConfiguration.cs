using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sis_Inmobiliaria.Core.Domain.Entities;

namespace Sis_Inmobiliaria.Infrastructure.Persistence.EntityConfigurations
{
    internal class PropertyEntityConfiguration
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Property");
            #endregion

            #region Property configurations
            builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
            #endregion

            #region relationships

            builder.HasOne(et => et.PropertyType)
                .WithMany(a => a.properties)
                .HasForeignKey(a => a.PropertyTypeId)
                .OnDelete(DeleteBehavior.Cascade);//lambda
            #endregion
        }
    }
}
