using System;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;

namespace MSC.Universal.Shared.UI.Implementation
{
    public class ViewModel : ViewModelBase
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnLoading();
                RaisePropertyChanged();
            }
        }

        protected virtual void OnLoading()
        {

        }

        public IAsyncAction BeginInvokeAsync(Action action)
        {
            return Window.Current.Dispatcher.RunAsync(new CoreDispatcherPriority(), () => action());
        }
    }
}
