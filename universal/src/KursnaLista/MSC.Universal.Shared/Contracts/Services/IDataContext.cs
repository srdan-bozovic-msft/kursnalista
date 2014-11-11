using System;

namespace MSC.Universal.Shared.Contracts.Services
{
    public interface IDataContext
    {
        string Context { get; set; }

        event EventHandler ContextChanged;

        string GetServerAddress();
    }
}
