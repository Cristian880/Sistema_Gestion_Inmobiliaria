using AutoMapper;
using Sis_Inmobiliaria.Core.Application.Dtos.PropertyTypeType;
using Sis_Inmobiliaria.Core.Application.ViewModels.PropertyTypeType;

namespace Sis_Inmobiliaria.Core.Application.Mappings.DtosAndViewModels
{
 public class PropertyTypeTypeDtoMappingProfile : Profile
 {
 public PropertyTypeTypeDtoMappingProfile()
 {
 CreateMap<PropertyTypeTypeDto, PropertyTypeTypeViewModel>()
 .ForMember(dest => dest.properties, opt => opt.MapFrom(src => src.Properties))
 .ReverseMap();

 CreateMap<PropertyTypeTypeDto, SavePropertyTypeTypeViewModel>()
 .ReverseMap();

 CreateMap<PropertyTypeTypeDto, DeletePropertyTypeTypeViewModel>()
 .ReverseMap()
 .ForAllMembers(opt => opt.Ignore())
 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
 }
 }
}
