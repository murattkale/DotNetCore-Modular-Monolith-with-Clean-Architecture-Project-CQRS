using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using dotnetcoreproject.Domain;
using Microsoft.EntityFrameworkCore;

public static class PagingHelper
{
    public static async Task<PagedResult<T>> GetPagedResultAsync<T>(
        IQueryable<T> query,
        int pageNumber,
        int pageSize,
        Expression<Func<T, object>> orderBy = null,
        bool orderByDescending = false,
        CancellationToken cancellationToken = default)
    {
        if (orderBy != null)
            query = orderByDescending
                ? query.OrderByDescending(orderBy)
                : query.OrderBy(orderBy);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>(items, totalCount, pageNumber, pageSize);
    }
}