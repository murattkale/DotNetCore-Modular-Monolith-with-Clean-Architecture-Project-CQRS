using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Infrastructure.Data;
using dotnetcoreproject.Infrastructure.Repositories;

namespace Infrastructure.Repositories;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly ICacheService _cacheService;
    private readonly ApplicationDbContext _context;
    private readonly IPropertyValueExtractor _propertyValueExtractor;
    private readonly ISessionService _sessionService;

    public RepositoryFactory(
        ApplicationDbContext context,
        ICacheService cacheService,
        ISessionService sessionService,
        IPropertyValueExtractor propertyValueExtractor)
    {
        _context = context;
        _cacheService = cacheService;
        _sessionService = sessionService;
        _propertyValueExtractor = propertyValueExtractor;
    }

    public IBaseRepository<T> CreateRepository<T>() where T : BaseEntity
    {
        return new EFRepository<T>(_context, _cacheService, _sessionService, _propertyValueExtractor);
    }
}