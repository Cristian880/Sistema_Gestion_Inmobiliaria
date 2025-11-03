using AutoMapper;
using Sis_Inmobiliaria.Core.Application.Dtos.PropertyType;
using Sis_Inmobiliaria.Core.Application.ViewModels.PropertyType;

namespace Sis_Inmobiliaria.Core.Application.Mappings.DtosAndViewModels
{
 public class PropertyTypeDtoMappingProfile : Profile
 {
 public PropertyTypeDtoMappingProfile()
 {
 CreateMap<PropertyTypeDto, PropertyTypeViewModel>()
 .ForMember(dest => dest.properties, opt => opt.MapFrom(src => src.Properties))
 .ReverseMap();

 CreateMap<PropertyTypeDto, SavePropertyTypeViewModel>()
 .ReverseMap();

 CreateMap<PropertyTypeDto, DeletePropertyTypeViewModel>()
 .ReverseMap()
 .ForAllMembers(opt => opt.Ignore())
 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
 }
 }
}
