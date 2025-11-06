using Sis_Inmobiliaria.Core.Domain.Common;

namespace Sis_Inmobiliaria.Core.Domain.Entities
{
    public class Property : BasicEntity<int>
    {
        public required string Name { get; set; }
        public required string Direction { get; set; }
        public required double price { get; set; }
        public required float size { get; set; }
        public string? PropertyImage { get; set; }

        //Datos que se mostraran cuando se precione la odcion de detalles de la propiedad
        public required string Description { get; set; }
        public required DateTime PropertyPublicacionDate { get; set; }
        public required bool Active { get; set; }

        //relaciones
        public required int PropertyTypeId { get; set; } //FK
        public PropertyType? PropertyType { get; set; }
        public string? UserId { get; set; } //FK
    }
}
