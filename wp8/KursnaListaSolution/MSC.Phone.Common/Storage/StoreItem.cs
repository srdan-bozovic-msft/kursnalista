using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Common.Storage
{
    public class StoreItem<T> : IStoreItem
    {
        public T Value { get; set; }
        public DateTime LastSync { get; set; }

        public void Set(T value)
        {
            Value = value;
            LastSync = DateTime.Now;
        }
    }
}
