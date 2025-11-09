using AutoMapper;
using Sis_Inmobiliaria.Core.Application.Dtos.PropertyType;
using Sis_Inmobiliaria.Core.Domain.Entities;
using System.Linq;

namespace Sis_Inmobiliaria.Core.Application.Mappings.EntitiesAndDtos
{
 public class PropertyTypeMappingProfile : Profile
 {
 public PropertyTypeMappingProfile()
 {
 CreateMap<PropertyType, PropertyTypeDto>()
 .ForMember(dest => dest.PropertyTypeType, opt => opt.MapFrom(src => src.PropertyTypeType))
 .ReverseMap()
 .ForMember(dest => dest.PropertyTypeType, opt => opt.Ignore());
 }
 }
}
