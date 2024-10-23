using dotnetcoreproject.Domain;

namespace Domain.Repositories;

public interface IRepositoryFactory
{
    IBaseRepository<T> CreateRepository<T>() where T : BaseEntity;
}