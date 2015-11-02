using System;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using MSC.Universal.Shared.UI.Contracts.ViewModels;

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

        public IErrorViewModel ErrorViewModel { get; private set; }

        public ViewModel()
        {
            ErrorViewModel = new ErrorViewModel();
        }

        protected virtual void OnLoading()
        {

        }

        public IAsyncAction BeginInvokeAsync(Action action)
        {
            return
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                    new CoreDispatcherPriority(), () => action()); 
            return Window.Current.Dispatcher.RunAsync(new CoreDispatcherPriority(), () => action());
        }
    }
}
