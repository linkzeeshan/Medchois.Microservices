using Medchois.UserManagementService.Domain.Contracts;

namespace Medchois.UserManagementService.Domain.Interfaces.IRepository
{
    public interface IGenericRepository<TEntity, TEntityId> where TEntity : BaseAuditableEntity<TEntityId>
    {
        IQueryable<TEntity> Queryable();

        Task<TEntity> GetByIdAsync(TEntityId id);

        Task InsertAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntityId id);
        Task<bool> SaveChangesAsync();
    }
}

