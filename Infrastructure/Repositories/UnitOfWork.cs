using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Domain.Repositories;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Infrastructure.Data;

namespace Infrastructure.Repositories;

public sealed class UnitOfWork(
    ApplicationDbContext context,
    IDbConnection dbConnection,
    IRepositoryFactory repositoryFactory)
    : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = new();

    private bool _disposed;

    public IBaseRepository<T> Repository<T>() where T : BaseEntity
    {
        if (_repositories.TryGetValue(typeof(T), out var repository))
            return (IBaseRepository<T>)repository;

        var efRepository = repositoryFactory.CreateRepository<T>();
        _repositories[typeof(T)] = efRepository;
        return efRepository;
    }

    public async Task<int> CompleteAsync()
    {
        return await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                context.Dispose();
                dbConnection.Dispose();
            }

            _disposed = true;
        }
    }
}