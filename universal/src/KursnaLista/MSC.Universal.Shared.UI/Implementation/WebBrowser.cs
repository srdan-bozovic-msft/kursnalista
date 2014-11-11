using System;
using System.Threading.Tasks;
using Windows.System;
using MSC.Universal.Shared.UI.Contracts.Services;

namespace MSC.Universal.Shared.UI.Implementation
{
    public class WebBrowser : IWebBrowser
    {
        public Task<bool> NavigateToAsync(Uri uri)
        {
            return Launcher.LaunchUriAsync(uri).AsTask();
        }
    }
}
