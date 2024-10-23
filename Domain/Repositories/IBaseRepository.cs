using System.Linq.Expressions;
using dotnetcoreproject.Domain;

namespace Domain.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, bool cache = true,
        bool asNoTracking = true, CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes);

    Task<T> GetByIdAsync(int id, bool cache = true, bool asNoTracking = true,
        CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes);

    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, bool cache = true,
        bool asNoTracking = true, CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes);

    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<T>> GetPagedAsync(PagedRequest<T> request);
}