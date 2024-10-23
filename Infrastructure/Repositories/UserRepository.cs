using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain.Entities;
using dotnetcoreproject.Infrastructure.Data;
using dotnetcoreproject.Infrastructure.Repositories;

namespace Infrastructure.Repositories;

public class UserRepository(
    ApplicationDbContext context,
    ICacheService cacheService,
    ISessionService sessionService,
    IPropertyValueExtractor propertyValueExtractor)
    : EFRepository<User>(context, cacheService, sessionService, propertyValueExtractor),
        IUserRepository;