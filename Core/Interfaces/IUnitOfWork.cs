using Core.Entities;

namespace Core.Interfaces;


// When UoW out of scope we can dispose 
public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity: BaseEntity;
    // Everyting ready or rooll it back
    Task<bool> Complete();
}
