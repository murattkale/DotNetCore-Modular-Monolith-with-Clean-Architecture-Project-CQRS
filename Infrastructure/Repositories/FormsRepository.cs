using Domain.Entities;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain.Entities;
using dotnetcoreproject.Infrastructure.Data;
using dotnetcoreproject.Infrastructure.Repositories;

namespace Infrastructure.Repositories;

public class FormsRepository(
    ApplicationDbContext context,
    ICacheService cacheService,
    ISessionService sessionService,
    IPropertyValueExtractor propertyValueExtractor)
    : EFRepository<Forms>(context, cacheService, sessionService, propertyValueExtractor),
        IFormsRepository;