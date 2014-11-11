using System;
using MSC.Universal.Shared.Contracts.Services;

namespace MSC.Universal.Shared.Implementation
{
    public class CacheItem<T> : ICacheItem<T>
    {
        private readonly T _value;
        private readonly bool _hasValue;
        private readonly DateTime _updated;
        private readonly DateTime _staleTime;
        private readonly DateTime _expirationTime;

        internal CacheItem()
            : this(false, default(T), DateTime.MinValue, DateTime.MinValue, DateTime.MinValue)
        {
            
        }

        internal CacheItem(T value, DateTime updated, DateTime staleTime, DateTime expirationTime)
            : this(true, value, updated, staleTime, expirationTime)
        {
            
        }
        
        private CacheItem(bool hasValue, T value, DateTime updated, DateTime staleTime, DateTime expirationTime)
        {
            _hasValue = hasValue;
            _value = value;
            _updated = updated;
            _staleTime = staleTime;
            _expirationTime = expirationTime;
        }

        public bool HasValue
        {
            get { return _hasValue; }
        }

        public T Value
        {
            get
            {
                return _value;
            }
        }

        public DateTime Updated
        {
            get { return _updated; }
        }


        public bool IsStale
        {
            get { return DateTime.Now > _staleTime; }
        }

        public bool IsExpired
        {
            get { return DateTime.Now > _expirationTime; }
        }
    }
}
