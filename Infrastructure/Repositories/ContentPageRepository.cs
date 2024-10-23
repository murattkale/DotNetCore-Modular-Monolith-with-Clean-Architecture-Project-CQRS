using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain.Entities;
using dotnetcoreproject.Infrastructure.Data;
using dotnetcoreproject.Infrastructure.Repositories;

namespace Infrastructure.Repositories;

public class ContentPageRepository(
    ApplicationDbContext context,
    ICacheService cacheService,
    ISessionService sessionService,
    IPropertyValueExtractor propertyValueExtractor)
    : EFRepository<ContentPage>(context, cacheService, sessionService, propertyValueExtractor),
        IContentPageRepository;