using System.Collections.Generic;
using Sis_Inmobiliaria.Core.Application.ViewModels;
using Sis_Inmobiliaria.Core.Application.ViewModels.PropertyType;

namespace Sis_Inmobiliaria.Core.Application.ViewModels.Property
{
     public class PropertyViewModel : BasicViewModel<int>
     {
         public required string Name { get; set; }
         public required string Direction { get; set; }
         public required double Price { get; set; }
         public required float Size { get; set; }
         public string? PropertyImage { get; set; }
         public required string Description { get; set; }
         public required DateTime PropertyPublicacionDate { get; set; }
         public required bool Active { get; set; }
         public required int PropertyTypeId { get; set; }
         public PropertyTypeViewModel? PropertyType { get; set; }
         public string? UserId { get; set; }
     }
}