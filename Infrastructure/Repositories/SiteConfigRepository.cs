using System.Threading;
using System.Threading.Tasks;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain.Entities;
using dotnetcoreproject.Infrastructure.Data;
using dotnetcoreproject.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SiteConfigRepository(
    ApplicationDbContext context,
    ICacheService cacheService,
    ISessionService sessionService,
    IPropertyValueExtractor propertyValueExtractor
)
    : EFRepository<SiteConfig>(context, cacheService, sessionService, propertyValueExtractor),
        ISiteConfigRepository
{
    private readonly ICacheService _cacheService = cacheService;

    public async Task<SiteConfig> GetByKeyAsync(string key, bool cache = false,
        CancellationToken cancellationToken = default)
    {
        if (cache)
        {
            var cacheKey = $"siteconfig_{key}";
            var duration = 1440;
            return await _cacheService.GetOrSetAsync(cacheKey,
                async () =>
                {
                    return await _context.SiteConfig.AsNoTracking()
                        .FirstOrDefaultAsync(c => c.ConfigKey == key, cancellationToken);
                }, duration);
        }

        return await _context.SiteConfig.AsNoTracking().FirstOrDefaultAsync(c => c.ConfigKey == key, cancellationToken);
    }
}