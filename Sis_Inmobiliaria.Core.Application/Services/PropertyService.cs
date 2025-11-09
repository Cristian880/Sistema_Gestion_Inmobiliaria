using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sis_Inmobiliaria.Core.Application.Dtos.Property;
using Sis_Inmobiliaria.Core.Application.Interfaces;
using Sis_Inmobiliaria.Core.Domain.Entities;
using Sis_Inmobiliaria.Core.Domain.Interfaces;

namespace Sis_Inmobiliaria.Core.Application.Services
{
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.

    public class PropertyService
        (
         IPropertyRepository propertyRepository,
         IMapper mapper,
         ILogger<GenericService<Property, PropertyDto>> logger
        ) : GenericService<Property, PropertyDto>(propertyRepository, mapper, logger), IPropertyService
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    {
        private readonly IPropertyRepository _propertyRepository = propertyRepository;
        //private readonly IPropertyRepository _propertyRepository;
        //private readonly IMapper _mapper;

        //public PropertyService(IPropertyRepository propertyRepository, IMapper mapper) : base(propertyRepository, mapper)
        //{
        //    _propertyRepository = propertyRepository;
        //    _mapper = mapper;
        //}

        public override async Task<PropertyDto?> GetById(int id)
        {
            try
            {
                var listEntitiesQuery = _propertyRepository.GetAllQueryWithInclude(["PropertyType"]);//(new List<string> { "PropertyType" });
                var entity = await listEntitiesQuery.FirstOrDefaultAsync(a => a.Id == id);

                if (entity == null)
                {
                    return null;
                }

                var dto = mapper.Map<PropertyDto>(entity);

                return dto;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting Property by id {PropertyId}", id);
                return null;
            }
        }

        public async Task<List<PropertyDto>> GetAllWithInclude()
        {
            try
            {
                var listEntitiesQuery = _propertyRepository.GetAllQueryWithInclude(["PropertyType"]);//(new List<string> { "PropertyType" });
                var listEntityDtos = await listEntitiesQuery.ProjectTo<PropertyDto>(mapper.ConfigurationProvider).ToListAsync();
                return listEntityDtos;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting all Properties with include");
                return [];//new List<PropertyDto>();
            }
        }

        public async Task<List<PropertyDto>> GetAllByUserIdAsync(string userId)
        {
            try
            {
                var listEntitiesQuery = _propertyRepository.GetAllQueryWithInclude(["PropertyType"])//(new List<string> { "PropertyType" });
                    .Where(p => p.UserId == userId);

                var listEntityDtos = await listEntitiesQuery.ProjectTo<PropertyDto>(mapper.ConfigurationProvider).ToListAsync();

                return listEntityDtos;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting Properties for UserId {UserId}", userId);
                return [];//new List<PropertyDto>();
            }
        }
    }
}
