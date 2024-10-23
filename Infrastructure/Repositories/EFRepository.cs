using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace dotnetcoreproject.Infrastructure.Repositories;

public class EFRepository<T> : IBaseRepository<T>, IDisposable where T : BaseEntity
{
    private readonly ICacheService _cacheService;
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;
    private readonly int _defaultCacheDuration;
    private readonly IPropertyValueExtractor _propertyValueExtractor;
    private readonly ISessionService _sessionService;
    private bool _disposed;

    public EFRepository(ApplicationDbContext context, ICacheService cacheService, ISessionService sessionService,
        IPropertyValueExtractor propertyValueExtractor)
    {
        _context = context;
        _dbSet = _context.Set<T>();
        _cacheService = cacheService;
        _sessionService = sessionService;
        _propertyValueExtractor = propertyValueExtractor;
        _defaultCacheDuration =
            CacheDuration().GetAwaiter().GetResult(); // Lazy Initialization for default cache duration
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        UpdateCacheForEntity(entity);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        UpdateCacheForEntities(entities);
    }

    public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, bool cache = true,
        bool asNoTracking = true, CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = ApplyIncludes(_dbSet, asNoTracking, includes);
        query = ApplyLangIdFilter(query);

      

        if (predicate != null) query = query.Where(predicate);


        var predicateString = "no_predicate";
        if (predicate != null)
        {
            var values = _propertyValueExtractor.ExtractValues(predicate);
            predicateString = string.Join("_", values);
        }

        var cacheKey = $"all_{typeof(T).Name.ToLower()}_{predicateString}_{_sessionService.GetLangId()}";
        if (!cache)
            _cacheService.Remove(cacheKey);

        var cachedData = await _cacheService.GetOrSetAsync(cacheKey,
            async () => await query.ToListAsync(cancellationToken), _defaultCacheDuration);
        return cachedData.AsQueryable();
    }

    public async Task<T> GetByIdAsync(int id, bool cache = true, bool asNoTracking = true,
        CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
    {
        var query = ApplyIncludes(_dbSet.Where(e => e.Id == id), asNoTracking, includes);
        query = ApplyLangIdFilter(query);

        var cacheKey = $"{typeof(T).Name.ToLower()}_{id}_{_sessionService.GetLangId()}";

        if (!cache)
            _cacheService.Remove(cacheKey);

        return await _cacheService.GetOrSetAsync(cacheKey,
            async () => await query.FirstOrDefaultAsync(cancellationToken), _defaultCacheDuration);
    }

    public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, bool cache = true,
        bool asNoTracking = true, CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = ApplyIncludes(_dbSet, asNoTracking, includes);
        
        query = ApplyLangIdFilter(query);

        if (predicate != null) query = query.Where(predicate);

        if (cache)
        {
            var predicateString = "no_predicate";
            if (predicate != null)
            {
                var values = _propertyValueExtractor.ExtractValues(predicate);
                predicateString = string.Join("_", values);
            }

            var cacheKey = $"single_{typeof(T).Name.ToLower()}_{predicateString}_{_sessionService.GetLangId()}";
            return await _cacheService.GetOrSetAsync(cacheKey,
                async () => await query.FirstOrDefaultAsync(cancellationToken), _defaultCacheDuration);
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        UpdateCacheForEntity(entity);
        return entity;
    }

    public async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbSet.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
        UpdateCacheForEntities(entities);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.DeletedDate = DateTime.UtcNow;
        await UpdateAsync(entity, cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities) entity.DeletedDate = DateTime.UtcNow;

        await UpdateRangeAsync(entities, cancellationToken);
    }

    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, false, true, cancellationToken);
        if (entity != null) await DeleteAsync(entity, cancellationToken);
    }

    public async Task<PagedResult<T>> GetPagedAsync(PagedRequest<T> request)
    {
        var query = ApplyIncludes(_dbSet, request.AsNoTracking, request.Includes);
        query = ApplyLangIdFilter(query);

        if (request.Predicate != null) query = query.Where(request.Predicate);


        var predicateString = "no_predicate";
        if (request.Predicate != null)
        {
            var values = _propertyValueExtractor.ExtractValues(request.Predicate);
            predicateString = string.Join("_", values);
        }

        var cacheKey =
            $"paged_{typeof(T).Name.ToLower()}_{predicateString}_{request.PageNumber}_{request.PageSize}_{_sessionService.GetLangId()}_{request.OrderBy}_{request.OrderByDescending}";

        if (request.Cache)
            _cacheService.Remove(cacheKey);

        var cachedData = await _cacheService.GetOrSetAsync(cacheKey,
            async () => await PagingHelper.GetPagedResultAsync(query, request.PageNumber, request.PageSize,
                request.OrderBy, request.OrderByDescending, new CancellationToken()), _defaultCacheDuration);
        return cachedData;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private async Task<int> CacheDuration()
    {
        var siteConfig = await _context.SiteConfig.AsNoTracking()
            .FirstOrDefaultAsync(c => c.ConfigKey == "DataCacheDuration");
        return siteConfig != null ? int.Parse(siteConfig.ConfigValue) : 1440;
    }

    private IQueryable<T> ApplyIncludes(IQueryable<T> query, bool asNoTracking,
        params Expression<Func<T, object>>[] includes)
    {
        if (asNoTracking) query = query.AsNoTracking();

        if (includes != null && includes.Any())
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        return query;
    }

    private IQueryable<T> ApplyLangIdFilter(IQueryable<T> query)
    {
        var entityName = typeof(T).Name;
        if (entityName == "Lang" || entityName == "User" || entityName == "LangDisplay"
            || entityName == "Documents" || entityName == "Forms"
           )
            return query;
        var langId = _sessionService.GetLangId();
        if (langId>0)
            query = query.Where(e => EF.Property<int?>(e, "LangId") == langId.Value);
        return query;
    }

    private void UpdateCacheForEntity(T entity)
    {
        var cacheKey = $"{typeof(T).Name.ToLower()}_{entity.Id}_{_sessionService.GetLangId()}";
        _cacheService.Remove(cacheKey);
    }

    private void UpdateCacheForEntities(IEnumerable<T> entities)
    {
        foreach (var entity in entities) UpdateCacheForEntity(entity);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing) _context.Dispose();

            _disposed = true;
        }
    }
}