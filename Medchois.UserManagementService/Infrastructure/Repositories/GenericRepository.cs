using Medchois.UserManagementService.Domain.Contracts;
using Medchois.UserManagementService.Domain.Interfaces.IRepository;
using System.Linq.Expressions;

namespace Medchois.UserManagementService.Infrastructure.Repositories
{
    public class GenericRepository<TEntity, TEntityId> : IGenericRepository<TEntity, TEntityId> where TEntity : BaseAuditableEntity<TEntityId>
    {
        public Task DeleteAsync(TEntityId id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync(TEntityId id)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Queryable()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
