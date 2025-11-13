using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Sis_Inmobiliaria.Core.Application.ViewModels.Property
{
     public class SavePropertyViewModel : BasicViewModel<int>
     {

        [Required(ErrorMessage = "Debe introducir el nombre de la propiedad")]
        [DataType(DataType.Text)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Debe introducir la dirección")]
        [DataType(DataType.Text)]
        public required string Direction { get; set; }

        [Required(ErrorMessage = "Debe introducir el precio")]
        [DataType(DataType.Text)]
        public double Price { get; set; }

        [Required(ErrorMessage = "Debe introducir el tamaño(metros)")]
        [DataType(DataType.Text)]
        public float Size { get; set; }

        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "You must enter the profile image of the property")]
        public IFormFile? PropertyImageFile { get; set; }

        [Required(ErrorMessage = "Debe introducir una descripcion")]
        [DataType(DataType.Text)]
        public required string Description { get; set; }

        //[Required(ErrorMessage = "Debe introducir una fecha de publicacion")]
        //[DataType(DataType.Text)]
        public required DateTime PropertyPublicacionDate { get; set; } = DateTime.UtcNow;

        [DataType(DataType.Text)]
        public required bool Active { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de propiedad válido")]
        public int? PropertyTypeId { get; set; }
        public string? UserId { get; set; }
     }
}