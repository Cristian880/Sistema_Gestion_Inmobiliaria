using System.ComponentModel.DataAnnotations;

namespace Sis_Inmobiliaria.Core.Application.ViewModels.PropertyType
{
     public class SavePropertyTypeViewModel : BasicViewModel<int>
    {
        [Required(ErrorMessage = "Debe introducir el nombre del tipo de propiedad")]
        [DataType(DataType.Text)]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Debe introducir la descripcion del tipo de propiedad")]
        [DataType(DataType.Text)]
        public required string Description { get; set; }
        public required bool Active { get; set; }
     }
}