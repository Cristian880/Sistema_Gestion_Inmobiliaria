using Sis_Inmobiliaria.Core.Application.ViewModels.Property;

namespace Sis_Inmobiliaria.Core.Application.ViewModels.PropertyType
{
     public class PropertyTypeViewModel : BasicViewModel<int>
     {
         public required string Name { get; set; }
         public required string Description { get; set; }
         public required bool Active { get; set; }
         public ICollection<PropertyViewModel>? properties { get; set; }
     }
}