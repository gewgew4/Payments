using Payments.Domain.Entities;
using System.Linq.Expressions;

namespace Payments.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Provides CRUD operations for the repository pattern
    /// </summary>
    /// <typeparam name="T">Class that derives from BaseEntity</typeparam>
    public interface IGenericRepository<T> where T : BaseEntity<T>
    {
        /// <summary>
        /// Adds an entity to the repositorry
        /// </summary>
        /// <param name="entity">Entity to be added</param>
        Task<T> Add(T entity);
        /// <summary>
        /// Get all entities from the repository
        /// </summary>
        /// <param name="includeProperties">Include nested/complex fields from the entity</param>
        /// <returns>List of all entities present at the repository</returns>
        Task<List<T>> GetAll(params string[] includeProperties);
        /// <summary>
        /// Get all entities following the specified conditions
        /// </summary>
        /// <param name="predicate">Linq conditions</param>
        /// <param name="orderBy">Order the results</param>
        /// <param name="top">Limits how many entities are returned</param>
        /// <param name="skip">Specifies hoy many entities will be skipped (offset)</param>
        /// <param name="includeProperties">Include nested/complex fields from the entity</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate,
                                      Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                      int? top = null,
                                      int? skip = null,
                                      params string[] includeProperties);
        /// <summary>
        /// Get a single entity from the repository
        /// </summary>
        /// <param name="id">Entity's id to be searched</param>
        /// <param name="tracking">If ORM should track changes</param>
        /// <param name="includeProperties">Include nested/complex fields from the entity</param>
        /// <returns></returns>
        Task<T> GetById(Guid id, bool tracking = true, params string[] includeProperties);
        /// <summary>
        /// Deletes an entity from the repository
        /// </summary>
        /// <param name="id">Entity's id to be removed</param>
        void Remove(Guid id);
        /// <summary>
        /// Deletes an entity from the repository
        /// </summary>
        /// <param name="entity">Entity to be removed</param>
        void Remove(T entity);
        /// <summary>
        /// Updates an entity from the repository
        /// </summary>
        /// <param name="entity">Entity to be updated</param>
        void Update(T entity);
    }
}
