using System;
using MSC.Phone.Shared.Contracts.Services;
using System.Threading.Tasks;

namespace MSC.Phone.Shared
{
	public class NullCacheService : ICacheService
	{
		#region ICacheService implementation

		public async Task<bool> ExistsAsync (string key)
		{
			return false;
		}

		public async Task PutAsync (string key, object value)
		{
		}

		public async Task<ICacheItem<T>> GetAsync<T> (string key)
		{
			return new CacheItem<T>(default(T), DateTime.MinValue);
		}

		public async Task<bool> HasBeenModifiedAsync (string key, DateTime since)
		{
			return true;
		}

		#endregion
	}
}

