using MSC.Phone.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Shared
{
    public class CacheItem<T> : ICacheItem<T>
    {
        private T _value;
        private DateTime _lastSync;

        internal CacheItem(T value, DateTime lastSync)
        {
            _value = value;
            _lastSync = lastSync;
        }

        public bool HasValue
        {
            get
            {
                return LastSync != DateTime.MinValue;
            }
        }

        public T Value
        {
            get
            {
                return _value;
            }
        }

        public DateTime LastSync
        {
            get { return _lastSync; }
        }
    }
}
