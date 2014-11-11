using Windows.Storage;
using MSC.Universal.Shared.Contracts.Services;

namespace MSC.Universal.Shared.Implementation
{
    public class SettingsService : ISettingsService
    {
        public void Set(string key, object value)
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values[key] = value;
        }

        public T Get<T>(string key)
        {
            return Get(key, default(T));
        }

        public T Get<T>(string key, T defaultValue)
        {
            var settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.ContainsKey(key))
            {
                return (T)settings.Values[key];
            }
            return defaultValue;
        }

        public bool ContainsKey(string key)
        {
            var settings = ApplicationData.Current.LocalSettings;
            return settings.Values.ContainsKey(key);
        }

        public void Remove(string key)
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values.Remove(key);
        }
    }
}
