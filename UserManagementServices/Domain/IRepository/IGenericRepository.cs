using System.Linq.Expressions;
using UserManagementServices.Domain.Common;

namespace UserManagementServices.Domain.IRepository
{
    public interface IGenericRepository<T> where T : IEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        void Add(T entity);
        void Update(T entity);
        Task<bool> SaveChangesAsync();
        Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate);
    }
}
