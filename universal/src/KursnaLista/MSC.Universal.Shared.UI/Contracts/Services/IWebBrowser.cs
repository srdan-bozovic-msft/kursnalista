using System;
using System.Threading.Tasks;

namespace MSC.Universal.Shared.UI.Contracts.Services
{
    public interface IWebBrowser
    {
        Task<bool> NavigateToAsync(Uri uri);
    }
}