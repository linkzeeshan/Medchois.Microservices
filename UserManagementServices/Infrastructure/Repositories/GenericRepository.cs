using System.Linq.Expressions;
using UserManagementServices.Domain.Common;
using UserManagementServices.Domain.IRepository;

namespace UserManagementServices.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : IEntity
    {
        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
