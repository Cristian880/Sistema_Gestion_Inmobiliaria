using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sis_Inmobiliaria.Core.Application.Dtos.Property;
using Sis_Inmobiliaria.Core.Application.Interfaces;
using Sis_Inmobiliaria.Core.Domain.Entities;
using Sis_Inmobiliaria.Core.Domain.Interfaces;

namespace Sis_Inmobiliaria.Core.Application.Services
{
    public class PropertyService : GenericService<Property, PropertyDto>, IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public PropertyService(IPropertyRepository propertyRepository, IMapper mapper) : base(propertyRepository, mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        public override async Task<PropertyDto?> GetById(int id)
        {
            try
            {
                var listEntitiesQuery = _propertyRepository.GetAllQueryWithInclude(new List<string> { "PropertyType" });

                var entity = await listEntitiesQuery.FirstOrDefaultAsync(a => a.Id == id);

                if (entity == null)
                {
                    return null;
                }

                var dto = _mapper.Map<PropertyDto>(entity);

                return dto;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<PropertyDto>> GetAllWithIncludeByUser(int userId)
        {
            try
            {
                var listEntitiesQuery = _propertyRepository.GetAllQueryWithInclude(new List<string> { "PropertyType" })
                    .Where(p => p.UserId == userId.ToString());

                var listEntityDtos = await listEntitiesQuery.ProjectTo<PropertyDto>(_mapper.ConfigurationProvider).ToListAsync();

                return listEntityDtos;
            }
            catch (Exception)
            {
                return new List<PropertyDto>();
            }
        }
    }
}
