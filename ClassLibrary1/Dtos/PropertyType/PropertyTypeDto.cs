using Sis_Inmobiliaria.Core.Application.Dtos.Property;

namespace Sis_Inmobiliaria.Core.Application.Dtos.PropertyType
{
    public class PropertyTypeDto : BasicDto<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required bool Active { get; set; }

        public ICollection<PropertyDto>? Properties { get; set; }
    }
}
