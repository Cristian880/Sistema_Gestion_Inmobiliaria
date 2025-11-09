using Sis_Inmobiliaria.Core.Application.Dtos.PropertyType;

namespace Sis_Inmobiliaria.Core.Application.Dtos.Property
{
    public class PropertyDto : BasicDto<int>
    {
        public required string Name { get; set; }
        public required string Direction { get; set; }
        public required double Price { get; set; }
        public required float size { get; set; }
        public string? PropertyImage { get; set; }
        //public required string PropertyBusisnessType { get; set; }


        //Datos que se mostraran cuando se precione la odcion de detalles de la propiedad
        public required string Description { get; set; }
        public required DateTime PropertyPublicacionDate { get; set; }
        public required bool Active { get; set; }

        //relaciones
        public required int PropertyTypeId { get; set; } //FK
        public PropertyTypeDto? PropertyType { get; set; }
        public string? UserId { get; set; } //FK
    }
}
