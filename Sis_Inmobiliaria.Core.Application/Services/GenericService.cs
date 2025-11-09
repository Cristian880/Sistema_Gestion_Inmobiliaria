using AutoMapper;
using Microsoft.Extensions.Logging;
using Sis_Inmobiliaria.Core.Application.Interfaces;
using Sis_Inmobiliaria.Core.Domain.Interfaces;

namespace Sis_Inmobiliaria.Core.Application.Services
{
    public class GenericService<Entity, DtoModel>(
        IGenericRepository<Entity> repository, 
        IMapper mapper,
        ILogger<GenericService<Entity, DtoModel>> logger) : IGenericService<DtoModel>
        where Entity : class
        where DtoModel : class
    {
        //private readonly IGenericRepository<Entity> _repository;
        //private readonly IMapper _mapper;

        //public GenericService(IGenericRepository<Entity> repository, IMapper mapper)
        //{
        //    _repository = repository;
        //    _mapper = mapper;
        //}
        public virtual async Task<DtoModel?> AddAsync(DtoModel dto)
        {
            try
            {
                Entity entity = mapper.Map<Entity>(dto);
                Entity? returnEntity = await repository.AddAsync(entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return mapper.Map<DtoModel>(returnEntity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al agregar la entidad {EntityName}", typeof(Entity).Name);
                return null;
            }
        }
        public virtual async Task<DtoModel?> UpdateAsync(DtoModel dto, int id)
        {
            try
            {
                Entity entity = mapper.Map<Entity>(dto);
                Entity? returnEntity = await repository.UpdateAsync(id, entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return mapper.Map<DtoModel>(returnEntity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al actualizar la entidad {EntityName} con Id {Id}", typeof(Entity).Name, id);
                return null;
            }
        }
        public virtual async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await repository.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al eliminar la entidad {EntityName} con Id {Id}", typeof(Entity).Name, id);
                return false;
            }
        }
        public virtual async Task<DtoModel?> GetById(int id)
        {
            try
            {
                var entity = await repository.GetById(id);
                if (entity == null)
                {
                    return null;
                }

                DtoModel dto = mapper.Map<DtoModel>(entity);
                return dto;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener la entidad {EntityName} por Id {Id}", typeof(Entity).Name, id);
                return null;
            }
        }
        public virtual async Task<List<DtoModel>> GetAll()
        {
            try
            {
                var listEntities = await repository.GetAllList();
                var listEntityDtos = mapper.Map<List<DtoModel>>(listEntities);

                return listEntityDtos;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener todas las entidades {EntityName}", typeof(Entity).Name);
                // 5. Advertencia de estilo corregida
                return [];
            }
        }
    }
}
