using dotnetcoreproject.Domain;

namespace Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IBaseRepository<T> Repository<T>() where T : BaseEntity;
    Task<int> CompleteAsync();
}