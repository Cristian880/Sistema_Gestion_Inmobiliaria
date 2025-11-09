using Sis_Inmobiliaria.Core.Application.Dtos.Property;

namespace Sis_Inmobiliaria.Core.Application.Interfaces
{
    public interface IPropertyService : IGenericService<PropertyDto>
    {
        Task<List<PropertyDto>> GetAllWithInclude();
        Task<List<PropertyDto>> GetAllByUserIdAsync(string userId);
    }
}
