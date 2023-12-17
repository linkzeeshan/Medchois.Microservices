using PatientManagementServices.Domain.Contracts;
using System.Linq.Expressions;

namespace PatientManagementServices.Domain.Interfaces.IRepository
{
    public interface IGenericRepository<TEntity,TEntityId> where TEntity : BaseAuditableEntity<TEntityId>
    {
        IQueryable<TEntity> Queryable();

        Task<TEntity> GetByIdAsync(TEntityId id);

        Task<TEntity> InsertAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntityId id);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(IEnumerable<TEntity> entities);
        Task<bool> SaveChangesAsync();
    }
}
