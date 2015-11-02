namespace MSC.Universal.Shared.Contracts.Services
{
    public interface ISettingsService
    {
        void Set(string key, object value);
        T Get<T>(string key);
        T Get<T>(string key, T defaultValue);
        bool ContainsKey(string key);
        void Remove(string key);
    }
}
