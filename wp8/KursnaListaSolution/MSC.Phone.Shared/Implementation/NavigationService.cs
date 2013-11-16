using Microsoft.Phone.Controls;
using MSC.Phone.Shared.Contracts.Services;
using MSC.Phone.Shared.Contracts.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MSC.Phone.Shared.Implementation
{
    public class NavigationService : INavigationService
    {
        private Frame _frame;
        public Frame Frame
        {
            get
            {
                return _frame;
            }
            set
            {
                _frame = value;
                _frame.Navigated += OnFrameNavigated;
            }
        }

        private IPageView CurrentView
        {
            get { return Frame.Content as IPageView; }
        }

        private void OnFrameNavigated(object sender, NavigationEventArgs e)
        {
            var view = e.Content as IPageView;
            if (view == null)
                return;

            var viewModel = view.ViewModel;
            if (viewModel != null)
            {
                if (!(e.NavigationMode == NavigationMode.Back
                    &&
                    (((Page)e.Content).NavigationCacheMode == NavigationCacheMode.Enabled
                    || (((Page)e.Content).NavigationCacheMode == NavigationCacheMode.Required))))
                {
                    //dynamic parameters = new object();
                    //((Page)e.Content).NavigationContext.QueryString
                    viewModel.InitializeAsync(e.Uri);
                }
            }
        }

        private void DisposePreviousView()
        {
            try
            {
                if (this.CurrentView != null
                    && ((Page)this.CurrentView).NavigationCacheMode == NavigationCacheMode.Disabled)
                {
                    var currentView = this.CurrentView;
                    var currentViewDisposable = currentView as IDisposable;

                    // if(currentView is BasePage
                    if (currentViewDisposable != null)
                    {
                        currentViewDisposable.Dispose();
                        currentViewDisposable = null;
                    } //view model is disposed in the view implementation
                    currentView = null;
                    GC.Collect();
                }
            }
            catch { }
        }

        public void Navigate(string pageKey)
        {
            NavigateTo(new Uri(string.Format("/{0}.xaml", pageKey), UriKind.Relative));
        }

        public void Navigate(string pageKey, object parameter)
        {
            DisposePreviousView();
            var key = Guid.NewGuid().ToString();
            //SimpleIoc.Default.Register<object>(() => parameter, key);

            NavigateTo(new Uri(string.Format("/{0}.xaml?key={1}", pageKey, key), UriKind.Relative));
        }

        private void NavigateTo(Uri pageUri)
        {
            Frame.Navigate(pageUri);
        }

        public void GoBack()
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
