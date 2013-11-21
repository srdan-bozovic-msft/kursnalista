using MSC.Phone.Shared.Contracts.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace MSC.Phone.Shared
{
    public class PhoneStorageCacheService : ICacheService
    {
        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                await localFolder.GetFileAsync(key).AsTask().ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            } 
        }

        public async Task PutAsync(string key, object value)
        {
            var mut = new Mutex(true, key);
            mut.WaitOne();
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var storageFile = await localFolder.CreateFileAsync(
                    key, CreationCollisionOption.ReplaceExisting);
                var json = JsonConvert.SerializeObject(value);

                using (var writer = new StreamWriter(await storageFile.OpenStreamForWriteAsync()))
                {
                    await writer.WriteAsync(json);
                }
            }
            catch
            {
            }
            finally
            {
                mut.ReleaseMutex();
            }
        }

        public async Task<ICacheItem<T>> GetAsync<T>(string key)
        {
            var mut = new Mutex(true, key);
            mut.WaitOne(); 
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;

                var file = await localFolder.GetFileAsync(key);

                var json = "";
                using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
                {
                    json = await reader.ReadToEndAsync();
                }

                return new CacheItem<T>(JsonConvert.DeserializeObject<T>(json), file.DateCreated.UtcDateTime);
            }
            catch
            {
                return new CacheItem<T>(default(T), DateTime.MinValue);
            }
            finally
            {
                mut.ReleaseMutex();
            }
        }


        public async Task<bool> HasBeenModifiedAsync(string key, DateTime since)
        {
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var file = await localFolder.GetFileAsync(key).AsTask().ConfigureAwait(false);
                return since.ToUniversalTime() < file.DateCreated.UtcDateTime;
            }
            catch
            {
                return false;
            } 
        }
    }
}
