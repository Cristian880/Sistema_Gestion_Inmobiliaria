using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sis_Inmobiliaria.Core.Application.Dtos.PropertyType;
using Sis_Inmobiliaria.Core.Application.Interfaces;
using Sis_Inmobiliaria.Core.Domain.Entities;
using Sis_Inmobiliaria.Core.Domain.Interfaces;


namespace Sis_Inmobiliaria.Core.Application.Services
{
    public class PropertyTypeService : GenericService<PropertyType, PropertyTypeDto>, IPropertyTypeService
    {
        private readonly IPropertyTypeRepository _PropertyTypeRepository;
        private readonly IMapper _mapper;
        public PropertyTypeService(IPropertyTypeRepository PropertyTypeRepository, IMapper mapper) : base(PropertyTypeRepository, mapper)
        {
            _PropertyTypeRepository = PropertyTypeRepository;
            _mapper = mapper;
        }

        public async Task<List<PropertyTypeDto>> GetAllWithInclude()
        {
            try
            {
                var listEntitiesQuery = _PropertyTypeRepository.GetAllQueryWithInclude(["Property"]);

                var listEntityDtos = await listEntitiesQuery.ProjectTo<PropertyTypeDto>(_mapper.ConfigurationProvider).ToListAsync();

                return listEntityDtos;
            }
            catch (Exception)
            {
                return [];
            }
        }
    }
}
