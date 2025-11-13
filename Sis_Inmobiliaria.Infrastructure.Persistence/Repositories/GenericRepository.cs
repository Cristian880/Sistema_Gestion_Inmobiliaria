using Microsoft.EntityFrameworkCore;
using Sis_Inmobiliaria.Infrastructure.Persistence.Contexts;
using Sis_Inmobiliaria.Core.Domain.Interfaces;
using System.Linq;

namespace Sis_Inmobiliaria.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<Entity> : IGenericRepository<Entity>
        where Entity : class
    {
        private readonly Sis_InmobiliariaAppContext _context;

        public GenericRepository(Sis_InmobiliariaAppContext context)
        {
            _context = context;
        }

        public virtual async Task<Entity?> AddAsync(Entity entity)
        {
            await _context.Set<Entity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public virtual async Task<List<Entity>?> AddRangeAsync(List<Entity> entities)
        {
            await _context.Set<Entity>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }
        public virtual async Task<Entity?> UpdateAsync(int id, Entity entity)
        {
            var entry = await _context.Set<Entity>().FindAsync(id);

            if (entry != null)
            {
                _context.Entry(entry).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return entry;
            }

            return null;
        }
        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<Entity>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<Entity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        public virtual async Task<List<Entity>> GetAllList()
        {
            return await _context.Set<Entity>().ToListAsync();
        }

        public virtual async Task<List<Entity>> GetAllListWithInclude(List<string> properties)
        {
            var query = _context.Set<Entity>().AsQueryable();

            if (properties == null || properties.Count == 0)
                return await query.ToListAsync();

            var entityType = _context.Model.FindEntityType(typeof(Entity));

            foreach (var property in properties)
            {
                if (string.IsNullOrWhiteSpace(property))
                    continue;

                // Verifica que la navegación exista antes de llamar Include
                if (entityType != null && entityType.FindNavigation(property) != null)
                {
                    query = query.Include(property);
                }
                // else: ignora include inválido (opcional: agregar logging aquí)
            }

            return await query.ToListAsync();
        }

        public virtual async Task<Entity?> GetById(int id)
        {
            return await _context.Set<Entity>().FindAsync(id);
        }
        public virtual IQueryable<Entity> GetAllQuery()
        {
            return _context.Set<Entity>().AsQueryable();
        }
        public virtual IQueryable<Entity> GetAllQueryWithInclude(List<string> properties)
        {
            var query = _context.Set<Entity>().AsQueryable();

            if (properties == null || properties.Count == 0)
                return query;

            var entityType = _context.Model.FindEntityType(typeof(Entity));

            foreach (var property in properties)
            {
                if (string.IsNullOrWhiteSpace(property))
                    continue;

                if (entityType != null && entityType.FindNavigation(property) != null)
                {
                    query = query.Include(property);
                }
            }

            return query;
        }
    }
}