using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain.Entities;
using dotnetcoreproject.Infrastructure.Data;

namespace dotnetcoreproject.Infrastructure.Repositories;

public class ContentPageImageRepository(
    ApplicationDbContext context,
    ICacheService cacheService,
    ISessionService sessionService,
    IPropertyValueExtractor propertyValueExtractor)
    : EFRepository<Documents>(context, cacheService, sessionService, propertyValueExtractor),
        IContentPageImageRepository;