namespace MSC.Universal.Shared.Contracts.Services
{
    public interface ICacheItem<out T>
    {
        bool HasValue { get; }
        T Value { get; }

        bool IsStale { get; }
        
        bool IsExpired { get; }
    }
}
