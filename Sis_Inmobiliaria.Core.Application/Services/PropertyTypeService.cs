using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sis_Inmobiliaria.Core.Application.Dtos.PropertyType;
using Sis_Inmobiliaria.Core.Application.Interfaces;
using Sis_Inmobiliaria.Core.Domain.Entities;
using Sis_Inmobiliaria.Core.Domain.Interfaces;


namespace Sis_Inmobiliaria.Core.Application.Services
{
    public class PropertyTypeService(
        IPropertyTypeRepository propertyTypeRepository,
        IMapper mapper,
        ILogger<GenericService<PropertyType, PropertyTypeDto>> logger
    ) : GenericService<PropertyType, PropertyTypeDto>(propertyTypeRepository, mapper, logger), IPropertyTypeService
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository = propertyTypeRepository;

        public async Task<List<PropertyTypeDto>> GetAllWithInclude()
        {
            try
            {
                var listEntitiesQuery = _propertyTypeRepository.GetAllQueryWithInclude(["Property"]);

                var listEntityDtos = await listEntitiesQuery.ProjectTo<PropertyTypeDto>(mapper.ConfigurationProvider).ToListAsync();

                return listEntityDtos;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error getting all PropertyTypes with include");
                return [];
            }
        }
    }
}
