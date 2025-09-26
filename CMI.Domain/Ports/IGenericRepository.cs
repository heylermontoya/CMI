using System.Linq.Expressions;

namespace CMI.Domain.Ports
{
    public interface IGenericRepository<E> : IDisposable
        where E : class
    {
        Task<IEnumerable<E>> GetAsync(Expression<Func<E, bool>>? filter = null,
                    Func<IQueryable<E>, IOrderedQueryable<E>>? orderBy = null, string includeStringProperties = "",
                    bool isTracking = false);

        Task<IEnumerable<E>> GetAsync(Expression<Func<E, bool>>? filter = null,
            Func<IQueryable<E>, IOrderedQueryable<E>>? orderBy = null,
             bool isTracking = false, params Expression<Func<E, object>>[] includeObjectProperties);

        IQueryable<E> GetQueryable(Expression<Func<E, bool>>? filter = null, string includeStringProperties = "");

        Task<E> FindByAlternateKeyAsync(Expression<Func<E, bool>> alternateKey, string includeProperties = "");

        Task<E?> GetByIdAsync(object id);
        Task<E> AddAsync(E entity);
        Task<IEnumerable<E>> AddRangeAsync(IEnumerable<E> entity);
        Task<E> UpdateAsync(E entity);
        Task<E> UpdateWithAsNoTrackingAsync(E entity);

        Task<IEnumerable<E>> UpdateRangeAsync(IEnumerable<E> entity);
        Task DeleteAsync(E entity);
        Task DeleteAsync(Expression<Func<E, bool>> filter);
    }
}
