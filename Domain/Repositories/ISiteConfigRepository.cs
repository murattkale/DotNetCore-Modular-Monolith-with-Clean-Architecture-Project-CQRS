using dotnetcoreproject.Domain.Entities;

namespace Domain.Repositories;

public interface ISiteConfigRepository : IBaseRepository<SiteConfig>
{
    Task<SiteConfig> GetByKeyAsync(string key, bool cache = false, CancellationToken cancellationToken = default);
}