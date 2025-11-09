using AutoMapper;
using Sis_Inmobiliaria.Core.Application.Dtos.PropertyTypeType;
using Sis_Inmobiliaria.Core.Domain.Entities;

namespace Sis_Inmobiliaria.Core.Application.Mappings.EntitiesAndDtos
{
 public class PropertyTypeTypeMappingProfile : Profile
 {
 public PropertyTypeTypeMappingProfile()
 {
 CreateMap<PropertyTypeType, PropertyTypeTypeDto>()
 .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.properties))
 .ReverseMap()
 .ForMember(dest => dest.properties, opt => opt.Ignore());
 }
 }
}
