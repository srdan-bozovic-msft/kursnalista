using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;

namespace MSC.Phone.Common.Storage
{
    public abstract class StoreBase
    {
        private readonly IsolatedStorageSettings _isolatedStore;
        private readonly string _storeName;
        private readonly Dictionary<string, IStoreItem> _items;

        protected abstract Task Load();

        private bool _isInitialized;
        private async Task InitializeIfNeeded()
        {
            if (!_isInitialized)
            {
                await Load();
                _isInitialized = true;
            }
        }

        protected async Task<T> GetValue<T>(string key)
        {
            await InitializeIfNeeded();
            if (!_items.ContainsKey(key))
                return default(T);
            return (_items[key] as StoreItem<T>).Value;
        }

        protected async Task<DateTime> GetLastSync(string key)
        {
            await InitializeIfNeeded();
            if (!_items.ContainsKey(key))
                return DateTime.MinValue;
            return _items[key].LastSync;
        }


        protected StoreBase(string storeName = "DefaultStore")
        {
            _storeName = storeName;
            _isolatedStore = IsolatedStorageSettings.ApplicationSettings;
            _items = new Dictionary<string, IStoreItem>();
        }

        protected async Task LoadComplexObject<T>(string key)
            where T : class, new()
        {
            await LoadComplexObject(key, new T());
        }

        protected async Task LoadComplexObject<T>(string key, T defaultValue)
            where T : class
        {
            StorageFile storageFile = null;
            var fileExists = false;
            var fileMutex = new Mutex(false, string.Format("{0}_{1}.store", _storeName, key));
            try
            {
                fileMutex.WaitOne();
                try
                {

                    // See if file exists
                    storageFile = await StorageFile.GetFileFromApplicationUriAsync(
                        new Uri(string.Format("ms-appdata:///local/{0}_{1}.store", _storeName, key)));
                    fileExists = true;
                }
                catch (FileNotFoundException)
                {
                    fileExists = false;
                }

                StoreItem<T> storeItem = null;

                if (fileExists)
                {
                    var readStream = await storageFile.OpenStreamForReadAsync();
                    using (var reader = new StreamReader(readStream))
                    {
                        try
                        {
                            var json = await reader.ReadToEndAsync();
                            if (!string.IsNullOrEmpty(json))
                            {
                                storeItem = JsonConvert.DeserializeObject<StoreItem<T>>(json);
                            }
                            else
                            {
                                storeItem = new StoreItem<T>();
                                storeItem.Value = defaultValue;
                            }
                        }
                        catch (Exception)
                        {
                            storeItem = new StoreItem<T>();
                            storeItem.Value = defaultValue;
                        }
                    }
                }
                else
                {
                    storeItem = new StoreItem<T>();
                    storeItem.Value = defaultValue;
                }

                _items[key] = storeItem;
            }
            finally
            {
                try
                {
                    fileMutex.ReleaseMutex();
                }
                catch
                {
                }
                fileMutex.Dispose();
            }
        }

        protected T LoadSimpleObject<T>(string key)
        {
            return LoadSimpleObject(key, default(T));
        }

        protected T LoadSimpleObject<T>(string key, T defaultValue)
        {
            lock (SyncRoot)
            {
                if (_isolatedStore.Contains(key))
                {
                    return (T)_isolatedStore[key];
                }
                return defaultValue;
            }
        }

        private static readonly object SyncRoot = new object();

        protected async Task StoreComplexObject<T>(string key, T value)
        {
            var storeItem = new StoreItem<T>();
            storeItem.Set(value);

            var fileMutex = new Mutex(false, string.Format("{0}_{1}.store", _storeName, key));
            try
            {
                fileMutex.WaitOne();
                var localFolder = ApplicationData.Current.LocalFolder;
                var storageFile = await localFolder.CreateFileAsync(string.Format("{0}_{1}.store", _storeName, key),
                                                                    CreationCollisionOption.OpenIfExists);

                var writeStream = await storageFile.OpenStreamForWriteAsync();
                using (var writer = new StreamWriter(writeStream))
                {
                    var json = JsonConvert.SerializeObject(storeItem);
                    await writer.WriteAsync(json);
                    _items[key] = storeItem;
                }
            }
            finally
            {
                try
                {
                    fileMutex.ReleaseMutex();
                }
                catch
                {
                }
                fileMutex.Dispose();
            }
        }

        protected void StoreSimpleObject(string key, object value)
        {
            lock (SyncRoot)
            {
                if (_isolatedStore.Contains(key))
                {
                    _isolatedStore[key] = value;
                }
                else
                {
                    _isolatedStore.Add(key, value);
                }
                _isolatedStore.Save();
            }
        }
    }
}
