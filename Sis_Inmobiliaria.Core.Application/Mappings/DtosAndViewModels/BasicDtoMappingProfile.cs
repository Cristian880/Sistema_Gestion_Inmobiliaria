using AutoMapper;
using Sis_Inmobiliaria.Core.Application.Dtos;
using Sis_Inmobiliaria.Core.Application.ViewModels;


namespace Sis_Inmobiliaria.Core.Application.Mappings.DtosAndViewModels
{
    public class BasicDtoMappingProfile : Profile
    {
        public BasicDtoMappingProfile()
        {

            CreateMap(typeof(BasicDto<>), typeof(BasicViewModel<>)).ReverseMap();
        }
    }
}
