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
            .ForMember(dest => dest.PropertyType, opt => opt.Ignore())
            .ForMember(dest => dest.Description, opt => opt.Ignore())
            .ForMember(dest => dest.price, opt => opt.Ignore())
            .ForMember(dest => dest.PropertyTypeId, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .ForMember(dest => dest.Active, opt => opt.Ignore())
            .ForMember(dest => dest.Direction, opt => opt.Ignore())
            .ForMember(dest => dest.PropertyImage, opt => opt.Ignore())
            .ForMember(dest => dest.size, opt => opt.Ignore())
            .ForMember(dest => dest.PropertyPublicacionDate, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore());
        }
    }
}