using System;
using System.Threading.Tasks;
using MSC.Universal.Shared.Contracts.Services;

namespace MSC.Universal.Shared.Implementation
{
    public class DummyCacheService : ICacheService
    {

        // ReSharper disable once CSharpWarnings::CS1998
        public async Task<bool> ExistsAsync<T>(string key)
        {
            return false;
        }

        // ReSharper disable once CSharpWarnings::CS1998
        public async Task PutAsync<T>(string key, T value, DateTime staleTime, DateTime expirationTime)
        {

        }

        // ReSharper disable once CSharpWarnings::CS1998
        public async Task PutAsync<T>(string key, DateTime updatedTime, T value, DateTime staleTime, DateTime expirationTime)
        {

        }

        // ReSharper disable once CSharpWarnings::CS1998
        public async Task UpdateAsync<T>(string key, T value)
        {

        }

        // ReSharper disable once CSharpWarnings::CS1998
        public async Task UpdateAsync<T>(string key, DateTime updatedTime, T value)
        {
        
        }

        // ReSharper disable once CSharpWarnings::CS1998
        public async Task<ICacheItem<T>> GetAsync<T>(string key)
        {
            return new CacheItem<T>();
        }

        // ReSharper disable once CSharpWarnings::CS1998
        public async Task<ICacheItem<T>> GetAsync<T>(string key, DateTime updatedTime)
        {
            return new CacheItem<T>();
        }

        // ReSharper disable once CSharpWarnings::CS1998
        public async Task RemoveAsync<T>(string key)
        {

        }

        // ReSharper disable once CSharpWarnings::CS1998
        public async Task CleanExpiredAsync()
        {

        }
    }
}
