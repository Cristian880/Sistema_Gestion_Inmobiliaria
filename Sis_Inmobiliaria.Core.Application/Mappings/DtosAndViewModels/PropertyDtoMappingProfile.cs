using AutoMapper;
using Sis_Inmobiliaria.Core.Application.Dtos.Property;
using Sis_Inmobiliaria.Core.Application.ViewModels.Property;

namespace Sis_Inmobiliaria.Core.Application.Mappings.DtosAndViewModels
{
     public class PropertyDtoMappingProfile : Profile
     {
         public PropertyDtoMappingProfile()
         {
             CreateMap<PropertyDto, PropertyViewModel>()
             .ForMember(dest => dest.PropertyType,
             opt => opt.MapFrom(src => src.PropertyType))
             .ReverseMap()
             .ForMember(dest => dest.PropertyType, opt => opt.Ignore());

             CreateMap<PropertyDto, SavePropertyViewModel>()
             .ReverseMap()
             .ForMember(dest => dest.PropertyType, opt => opt.Ignore());

             CreateMap<PropertyDto, DeletePropertyViewModel>()
             .ReverseMap()
             .ForMember(dest => dest.PropertyType, opt => opt.Ignore());
         }
     }
}