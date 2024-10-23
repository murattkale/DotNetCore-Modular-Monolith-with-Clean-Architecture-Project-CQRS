using System;
using System.Threading.Tasks;

namespace dotnetcoreproject.Application.Interfaces;

public interface ICacheService
{
    Task<T> GetOrSetAsync<T>(string cacheKey, Func<Task<T>> getItemCallback, int cacheDuration) where T : class;
    void Remove(string cacheKey);
}