using AutoMapper;
using Sis_Inmobiliaria.Core.Application.Dtos.PropertyType;
using Sis_Inmobiliaria.Core.Application.ViewModels.PropertyType;

namespace Sis_Inmobiliaria.Core.Application.Mappings.DtosAndViewModels
{
 public class PropertyTypeDtoMappingProfile : Profile
 {
 public PropertyTypeDtoMappingProfile()
 {
 // Full view model (show/details) - include PropertyTypeType
 CreateMap<PropertyTypeDto, PropertyTypeViewModel>()
 .ForMember(dest => dest.PropertyTypeType, opt => opt.MapFrom(src => src.PropertyTypeType))
 .ReverseMap(); // al editar no ignorar nada, mapear todos los campos disponibles

 // Save (create/edit) - no mapear IFormFile (ImageFile) porque no existe en DTO; ImageFile se maneja en la UI
 CreateMap<PropertyTypeDto, SavePropertyTypeViewModel>()
 .ReverseMap(); // permitir mapear todos los datos desde el VM al DTO (el ImageFile se procesa en la capa de presentación)

 // Delete - al mapear de DeleteViewModel a DTO sólo considerar el Id
 CreateMap<PropertyTypeDto, DeletePropertyTypeViewModel>()
 .ReverseMap()
 .ForAllMembers(opt => opt.Ignore())
 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
 }
 }
}
