using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Shared.Contracts.Services
{
    public interface ICacheItem<T>
    {
        bool HasValue { get; }
        T Value
        {
            get;
        }
        DateTime LastSync { get; }
    }
}
