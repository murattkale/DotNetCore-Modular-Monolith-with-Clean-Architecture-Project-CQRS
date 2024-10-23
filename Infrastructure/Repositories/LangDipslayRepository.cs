using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain.Entities;
using dotnetcoreproject.Infrastructure.Data;
using dotnetcoreproject.Infrastructure.Repositories;

namespace Infrastructure.Repositories;

public class LangDisplayRepository(
    ApplicationDbContext context,
    ICacheService cacheService,
    ISessionService sessionService,
    IPropertyValueExtractor propertyValueExtractor)
    : EFRepository<LangDisplay>(context, cacheService, sessionService, propertyValueExtractor),
        ILangDisplayRepository;