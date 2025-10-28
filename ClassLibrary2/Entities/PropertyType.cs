using Sis_Inmobiliaria.Core.Domain.Common;
using System.Collections;

namespace Sis_Inmobiliaria.Core.Domain.Entities
{
    public class PropertyType : BasicEntity<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required bool Active { get; set; }

        public ICollection<Property> properties { get; set; }
    }
}
