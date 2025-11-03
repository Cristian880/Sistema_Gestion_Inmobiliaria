using AutoMapper;
using Sis_Inmobiliaria.Core.Application.Dtos.Property;
using Sis_Inmobiliaria.Core.Application.ViewModels.Property;

namespace Sis_Inmobiliaria.Core.Application.Mappings.DtosAndViewModels
{
 public class PropertyDtoMappingProfile : Profile
 {
 public PropertyDtoMappingProfile()
 {
 // Full view model (show/details) - include PropertyType
 CreateMap<PropertyDto, PropertyViewModel>()
 .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType))
 .ReverseMap(); // al editar no ignorar nada, mapear todos los campos disponibles

 // Save (create/edit) - no mapear IFormFile (ImageFile) porque no existe en DTO; ImageFile se maneja en la UI
 CreateMap<PropertyDto, SavePropertyViewModel>()
 .ReverseMap(); // permitir mapear todos los datos desde el VM al DTO (el ImageFile se procesa en la capa de presentación)

 // Delete - al mapear de DeleteViewModel a DTO sólo considerar el Id
 CreateMap<PropertyDto, DeletePropertyViewModel>()
 .ReverseMap()
 .ForAllMembers(opt => opt.Ignore())
 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
 }
 }
}
