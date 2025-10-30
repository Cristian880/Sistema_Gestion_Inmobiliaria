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
             .ForMember(dest => dest.properties,
             opt => opt.MapFrom(src => src.Properties))
             .ReverseMap()
             .ForMember(dest => dest.Properties, opt => opt.Ignore());

             CreateMap<PropertyTypeDto, SavePropertyTypeViewModel>()
             .ReverseMap();

             CreateMap<PropertyTypeDto, DeletePropertyTypeViewModel>()
             .ReverseMap();
         }
     }
}