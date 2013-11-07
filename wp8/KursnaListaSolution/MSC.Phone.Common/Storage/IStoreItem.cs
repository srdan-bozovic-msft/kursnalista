using System;

namespace MSC.Phone.Common.Storage
{
    public interface IStoreItem
    {
        DateTime LastSync { get; }
    }
}