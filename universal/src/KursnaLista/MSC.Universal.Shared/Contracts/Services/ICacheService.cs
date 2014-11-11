using System;
using System.Threading.Tasks;

namespace MSC.Universal.Shared.Contracts.Services
{
    public interface ICacheService
    {
        Task<bool> ExistsAsync<T>(string key);
        Task PutAsync<T>(string key, T value, DateTime staleTime, DateTime expirationTime);
        Task PutAsync<T>(string key, DateTime updatedTime, T value, DateTime staleTime, DateTime expirationTime);
        Task UpdateAsync<T>(string key, T value);
        Task UpdateAsync<T>(string key, DateTime updatedTime, T value); 
        Task<ICacheItem<T>> GetAsync<T>(string key);
        Task<ICacheItem<T>> GetAsync<T>(string key, DateTime updatedTime);
        Task RemoveAsync<T>(string key);
        Task CleanExpiredAsync();
    }
}
