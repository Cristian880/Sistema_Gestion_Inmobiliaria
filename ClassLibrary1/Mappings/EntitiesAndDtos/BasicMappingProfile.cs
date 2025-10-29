using AutoMapper;
using Sis_Inmobiliaria.Core.Application.Dtos;
using Sis_Inmobiliaria.Core.Domain.Common;

namespace Sis_Inmobiliaria.Core.Application.Mappings.EntitiesAndDtos
{
    public class BasicMappingProfile : Profile
    {
        public BasicMappingProfile()
        {
            CreateMap(typeof(BasicDto<>), typeof(BasicEntity<>)).ReverseMap();
        }
    }
}
