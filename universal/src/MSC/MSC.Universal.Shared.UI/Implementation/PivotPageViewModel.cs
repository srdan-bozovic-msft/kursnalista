using System;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;

namespace MSC.Universal.Shared.UI.Implementation
{
    public abstract class PivotPageViewModel : ViewModelBase
    {
        private string _title;

        public string Title
        {
            get { return _title != null? _title.ToLower() : string.Empty; }
            set { Set(()=>Title, ref _title, value); }
        }

        public IAsyncAction BeginInvokeAsync(Action action)
        {
            return
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                    new CoreDispatcherPriority(), () => action());
        }
    }
}