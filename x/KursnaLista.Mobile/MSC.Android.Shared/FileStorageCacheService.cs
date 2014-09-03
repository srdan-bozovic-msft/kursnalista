using MSC.Phone.Shared.Contracts.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSC.Phone.Shared;

namespace MSC.Android.Shared
{
    public class FileStorageCacheService : ICacheService
    {
        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
				var localFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				var path = Path.Combine(localFolder, key);
				return File.Exists(path);
            }
            catch
            {
                return false;
            } 
        }

        public async Task PutAsync(string key, object value)
        {
            try
            {
				var localFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				if(!Directory.Exists(localFolder))
					Directory.CreateDirectory(localFolder);

				var path = Path.Combine(localFolder, key);

				var json = JsonConvert.SerializeObject(value);

				File.WriteAllText(path, json);
            }
			catch (Exception xcp)
            {
            }
        }

        public async Task<ICacheItem<T>> GetAsync<T>(string key)
        {
            try
            {
				var localFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				var path = Path.Combine(localFolder, key);

				if(!File.Exists(path))
				{
					return new CacheItem<T>(default(T), DateTime.MinValue);
				}

				var json = File.ReadAllText(path);

				var fileInfo = new FileInfo(path);

                return new CacheItem<T>(JsonConvert.DeserializeObject<T>(json), fileInfo.CreationTimeUtc);
            }
			catch (Exception xcp)
            {
                return new CacheItem<T>(default(T), DateTime.MinValue);
            }
        }


        public async Task<bool> HasBeenModifiedAsync(string key, DateTime since)
        {
            try
            {
				var localFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				var path = Path.Combine(localFolder, key);

				if(!File.Exists(path))
				{
					return false;
				}

				var fileInfo = new FileInfo(path);

				return since.ToUniversalTime() < fileInfo.CreationTimeUtc;
            }
            catch
            {
                return false;
            } 
        }
    }
}
