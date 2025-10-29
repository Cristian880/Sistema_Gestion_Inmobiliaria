using Sis_Inmobiliaria.Core.Application.Dtos.PropertyType;

namespace Sis_Inmobiliaria.Core.Application.Interfaces
{
    public interface IPropertyTypeService : IGenericService<PropertyTypeDto>
    {
        Task<List<PropertyTypeDto>> GetAllWithInclude();
    }
}
