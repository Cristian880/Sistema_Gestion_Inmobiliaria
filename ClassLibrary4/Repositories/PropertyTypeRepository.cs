using Sis_Inmobiliaria.Core.Domain.Entities;
using Sis_Inmobiliaria.Core.Domain.Interfaces;
using Sis_Inmobiliaria.Infrastructure.Persistence.Contexts;

namespace Sis_Inmobiliaria.Infrastructure.Persistence.Repositories
{
    public class PropertyTypeRepository : GenericRepository<PropertyType>, IPropertyTypeRepository
    {
        public PropertyTypeRepository(Sis_InmobiliariaAppContext context) : base(context)
        {
        }
    }
}
