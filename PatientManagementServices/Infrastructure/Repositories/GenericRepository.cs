using Microsoft.EntityFrameworkCore;
using PatientManagementServices.Domain.Contracts;
using PatientManagementServices.Domain.Entities;
using PatientManagementServices.Domain.Interfaces.IRepository;
using PatientManagementServices.Infrastructure.Data;
using System.Diagnostics;
using System.Linq.Expressions;

namespace PatientManagementServices.Infrastructure.Repositories
{
    /// <summary>
    /// Generic Repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityId"></typeparam>
    public class GenericRepository<TEntity, TEntityId> : IGenericRepository<TEntity, TEntityId> where TEntity : BaseAuditableEntity<TEntityId>
    {
        #region Fields
        protected readonly ApplicationDbContext _dbContext;
        private DbSet<TEntity> _table = null;
        #endregion

        #region Ctor
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext)); ;
            _table = _dbContext.Set<TEntity>();
        }
        #endregion

        #region Function Implementation
        /// <summary>
        /// Generic Table we can Implement any query on it
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> Queryable()
        {
            return _table.AsNoTracking().AsQueryable();
        }
        /// <summary>
        /// Get entity by entity id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> GetByIdAsync(TEntityId id)
        {
            return await _table.FindAsync(id);
        }
        /// <summary>
        /// Insert entity 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _table.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        /// <summary>
        /// update entity 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            // Detach the existing tracked entity
            _table.Entry(entity).State = EntityState.Detached;

            // Attach the updated entity and mark it as modified
            _table.Attach(entity);
            _table.Entry(entity).State = EntityState.Modified;
            await SaveChangesAsync();
            // Reload the entity from the database to get the latest state
            await _table.Entry(entity).ReloadAsync();

            // Return the updated entity
            return entity;
        }
        /// <summary>
        /// Delete entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(TEntityId id)
        {
            var entityToDelete = await _table.FindAsync(id);
            if (entityToDelete != null)
            {
                _table.Remove(entityToDelete);
                await SaveChangesAsync();
            }
        }
        /// <summary>
        /// Delete entity by id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task DeleteAsync(TEntity entity)
        {
            if (entity != null)
            {
                _table.Remove(entity);
                await SaveChangesAsync();
            }
        }
        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _table.RemoveRange(entities.ToList());
            await SaveChangesAsync();
        }
        /// <summary>
        /// entity save changes in table
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbContext.SaveChangesAsync() >= 0);
        }
        /// <summary>
        /// entity update
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task Update(TEntity entity)
        {
            if (_table == null)
            {
                throw new ArgumentNullException("entity");
            }
             _table.Update(entity);
            await SaveChangesAsync();
        }
        #endregion
    }
}
