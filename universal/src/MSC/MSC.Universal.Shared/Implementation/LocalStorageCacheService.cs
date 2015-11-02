using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using MSC.Universal.Shared.Contracts.Services;
using Newtonsoft.Json;

namespace MSC.Universal.Shared.Implementation
{
    public class LocalStorageCacheService : ICacheService, IDisposable
    {
        private const char FileNameSeparator = '»';

        private readonly IDataContext _cacheContext;
        private readonly List<string> _files;
        private IStorageFolder _root;

        private static object Sync = new object();

        public LocalStorageCacheService(IDataContext cacheContext)
        {
            _cacheContext = cacheContext;
            _cacheContext.ContextChanged += ContextChanged;
            _files = new List<string>();
        }

        #region EnsureRoot
        public void Dispose()
        {
            _cacheContext.ContextChanged -= ContextChanged;
        }

        private async void ContextChanged(object sender, EventArgs e)
        {
            if (_root != null &&
                _cacheContext.Context != _root.Name)
            {
                await InitializeRootAsync().ConfigureAwait(false);
            }
        }

        private async Task EnsureRootAsync()
        {
            if (_root != null)
                return;
            await InitializeRootAsync().ConfigureAwait(false);
        }

        private async Task InitializeRootAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var folders = await localFolder.GetFoldersAsync().AsTask().ConfigureAwait(false);
            if (_root != null)
            {
                var previousRoot = folders.FirstOrDefault(f => f.Name == _root.Name);
                if (previousRoot != null)
                    await previousRoot.DeleteAsync().AsTask().ConfigureAwait(false);
            }
            _root = folders.FirstOrDefault(f => f.Name == _cacheContext.Context);
            if (_root == null)
            {
                _root = await localFolder.CreateFolderAsync(_cacheContext.Context).AsTask().ConfigureAwait(false);
            }
                     
            var files = (await _root.GetFilesAsync().AsTask().ConfigureAwait(false))
                .Select(f => f.Name);

            var expired = new List<string>();
            lock (Sync)
            {
                _files.Clear();
                foreach (var file in files)
                {
                    if (IsExpired(file))
                    {
                        expired.Add(file);
                    }
                    else
                    {
                        _files.Add(file);
                    }
                }
            }

            // ReSharper disable once CSharpWarnings::CS4014, no need to wait storage operation
            RemoveFilesAsync(expired);
        }

        #endregion

        public async Task<bool> ExistsAsync<T>(string key)
        {
            try
            {
                await EnsureRootAsync().ConfigureAwait(false);

                return FindFileName<T>(key) != null;
            }
            catch
            {
                return false;
            } 
        }

        private string FindFileName<T>(string key)
        {
            return
                FindFileName(
                    f => f.StartsWith(string.Format("{2}{0}{1}{0}", FileNameSeparator, key, typeof(T).Name))
                    );
        }

        private string FindFileName<T>(string key, DateTime updatedTime)
        {
            return
                FindFileName(
                    f => f.StartsWith(string.Format("{2}{0}{1}{0}{3}{0}", FileNameSeparator, key, typeof (T).Name, updatedTime.Ticks))
                    );
        }

        private string FindFileName(Func<string, bool> query)
        {
            lock (Sync)
            {
                var results = _files
                    .Where(query)
                    .OrderByDescending(f => f);
                if (results.Any())
                    return results.First();
                return null;
            }
        }

        public Task PutAsync<T>(string key, T value, DateTime staleTime, DateTime expirationTime)
        {
            return PutAsync(key, DateTime.MinValue, value, staleTime, expirationTime);
        }

        public async Task PutAsync<T>(string key, DateTime updatedTime, T value, DateTime staleTime, DateTime expirationTime)
        {
            try
            {
                await EnsureRootAsync().ConfigureAwait(false);

                await RemoveAsync<T>(key).ConfigureAwait(false);

                var fileName = string.Format("{2}{0}{1}{0}{3}{0}{4}{0}{5}{0}.txt",
                    FileNameSeparator, 
                    key,
                    value.GetType().Name,
                    updatedTime.Ticks,
                    staleTime.Ticks,
                    expirationTime.Ticks);

                var storageFile = await _root.CreateFileAsync(
                    fileName, CreationCollisionOption.ReplaceExisting).AsTask().ConfigureAwait(false);
                
                var json = JsonConvert.SerializeObject(value);

                using (var writer = new StreamWriter(await storageFile.OpenStreamForWriteAsync().ConfigureAwait(false)))
                {
                    await writer.WriteAsync(json).ConfigureAwait(false);
                }
                lock (Sync)
                {
                    _files.Add(storageFile.Name);
                }
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }

        public Task UpdateAsync<T>(string key, T value)
        {
            return UpdateAsync(() => FindFileName<T>(key), value);
        }

        public Task UpdateAsync<T>(string key, DateTime updatedTime, T value)
        {
            return UpdateAsync(() => FindFileName<T>(key, updatedTime), value);
        }

        private async Task UpdateAsync<T>(Func<string> findFileFunc, T value)
        {
            try
            {
                await EnsureRootAsync().ConfigureAwait(false);

                var fileName = findFileFunc();

                if (!string.IsNullOrEmpty(fileName))
                {
                    var storageFile = await _root.CreateFileAsync(
                        fileName, CreationCollisionOption.ReplaceExisting).AsTask().ConfigureAwait(false);

                    var json = JsonConvert.SerializeObject(value);

                    using (
                        var writer = new StreamWriter(await storageFile.OpenStreamForWriteAsync().ConfigureAwait(false))
                        )
                    {
                        await writer.WriteAsync(json).ConfigureAwait(false);
                    }
                }
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }


        public Task<ICacheItem<T>> GetAsync<T>(string key)
        {
            return GetAsync<T>(() => FindFileName<T>(key));            
        }

        public Task<ICacheItem<T>> GetAsync<T>(string key, DateTime updatedTime)
        {
            return GetAsync<T>(() => FindFileName<T>(key, updatedTime));
        }

        private async Task<ICacheItem<T>> GetAsync<T>(Func<string> findFileFunc)
        {
            try
            {
                await EnsureRootAsync().ConfigureAwait(false);

                var fileName = findFileFunc();

                if (string.IsNullOrEmpty(fileName))
                    return new CacheItem<T>();

                var file = await _root.GetFileAsync(fileName).AsTask().ConfigureAwait(false);

                string json;
                using (var reader = new StreamReader(await file.OpenStreamForReadAsync().ConfigureAwait(false)))
                {
                    json = await reader.ReadToEndAsync().ConfigureAwait(false);
                }

                var meta = fileName.Split(FileNameSeparator);

                return new CacheItem<T>(
                    JsonConvert.DeserializeObject<T>(json),
                    new DateTime(long.Parse(meta[2])),
                    new DateTime(long.Parse(meta[3])),
                    new DateTime(long.Parse(meta[4]))
                    );
            }
            catch
            {
                return new CacheItem<T>();
            }
        }

        public async Task CleanExpiredAsync()
        {
            IEnumerable<string> filesToRemove;
            lock (Sync)
            {
                filesToRemove = _files.Where(IsExpired).ToArray();
            }
            await RemoveFilesAsync(filesToRemove).ConfigureAwait(false);
        }

        private static bool IsExpired(string file)
        {
            var meta = file.Split(FileNameSeparator);
            return DateTime.Now > new DateTime(long.Parse(meta[4]));
        }

        public async Task RemoveAsync<T>(string key)
        {
            IEnumerable<string> filesToRemove;
            lock (Sync)
            {
                filesToRemove = _files.Where(f => f.StartsWith(string.Format("{2}{0}{1}{0}", FileNameSeparator, key, typeof(T).Name))).ToArray();
            }
            await RemoveFilesAsync(filesToRemove).ConfigureAwait(false);
        }

        private async Task RemoveFilesAsync(IEnumerable<string> filesToRemove)
        {
            foreach (var file in filesToRemove)
            {
                var storedFile = await _root.GetFileAsync(file).AsTask().ConfigureAwait(false);
                await storedFile.DeleteAsync().AsTask().ConfigureAwait(false);
                lock (Sync)
                {
                    _files.Remove(file);
                }
            }
        }
    }
}
