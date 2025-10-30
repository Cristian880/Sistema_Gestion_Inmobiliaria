using AutoMapper;
using Sis_Inmobiliaria.Core.Application.Dtos.Property;
using Sis_Inmobiliaria.Core.Domain.Entities;

namespace Sis_Inmobiliaria.Core.Application.Mappings.EntitiesAndDtos
{
     public class PropertyMappingProfile : Profile
     {
         public PropertyMappingProfile()
         {
             CreateMap<Property, PropertyDto>()
             .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType))
             .ReverseMap()
             .ForMember(dest => dest.PropertyType, opt => opt.Ignore());

             CreateMap<Property, PropertyDto>()
             .ReverseMap();
         }
     }
}