using AutoMapper;
using Sis_Inmobiliaria.Core.Application.Dtos.PropertyType;
using Sis_Inmobiliaria.Core.Domain.Entities;

namespace Sis_Inmobiliaria.Core.Application.Mappings.EntitiesAndDtos
{
     public class PropertyTypeMappingProfile : Profile
     {
         public PropertyTypeMappingProfile()
         {
             CreateMap<PropertyType, PropertyTypeDto>()
             .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.Properties))
             .ReverseMap()
             .ForMember(dest => dest.Properties, opt => opt.Ignore());
         }
     }
}