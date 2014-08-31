//using Microsoft.Phone.Controls;
//using Microsoft.Phone.Shell;
//using MSC.Phone.Shared.Contracts.Services;
//using MSC.Phone.Shared.Contracts.Views;
//using System;
//using System.Collections.Generic;
//using System.Dynamic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Navigation;
//
//namespace MSC.Phone.Shared
//{
//    public class NavigationService : INavigationService
//    {
//        private class DynamicDictionary : DynamicObject
//        {
//            private readonly IDictionary<string, string> _properties;
//
//            public DynamicDictionary(IDictionary<string,string> properties)
//            {
//                _properties = properties;
//            }
//
//            public override bool TryGetMember(GetMemberBinder binder, out object result)
//            {
//                result = null;
//                if (!_properties.ContainsKey(binder.Name))
//                    return false;
//                var value = _properties[binder.Name];
//                result = Convert.ChangeType(value, binder.ReturnType);
//                return true;
//            }
//        }
//
//        private Dictionary<string, object> _parameters;
//
//        private Frame _frame;
//        public Frame Frame
//        {
//            get
//            {
//                return _frame;
//            }
//            set
//            {
//                _frame = value;
//                _frame.Navigated += OnFrameNavigated;
//            }
//        }
//
//        private IPageView CurrentView
//        {
//            get { return Frame.Content as IPageView; }
//        }
//
//        public NavigationService()
//        {
//            _parameters = new Dictionary<string, object>();
//        }
//
//        private async void OnFrameNavigated(object sender, NavigationEventArgs e)
//        {
//            var view = e.Content as IPageView;
//            if (view == null)
//                return;
//
//            var viewModel = view.ViewModel;
//            if (viewModel != null)
//            {
//                if (!(e.NavigationMode == NavigationMode.Back
//                    &&
//                    (((Page)e.Content).NavigationCacheMode == NavigationCacheMode.Enabled
//                    || (((Page)e.Content).NavigationCacheMode == NavigationCacheMode.Required))))
//                {
//                    var parameters = ((Page)e.Content).NavigationContext.QueryString;
//                    if (parameters.Count == 1 && parameters.ContainsKey("x-guid"))
//                    {
//                        var key = parameters["x-guid"];
//                        if (_parameters.ContainsKey(key))
//                        {
//                            var parameter = _parameters[key];
//                            _parameters.Remove(key);
//                            await viewModel.InitializeAsync(parameter);
//                        }
//                    }
//                    else
//                    {
//                        await viewModel.InitializeAsync(new DynamicDictionary(parameters));
//                    }
//                }
//            }
//        }
//
//        private void DisposePreviousView()
//        {
//            try
//            {
//                if (this.CurrentView != null
//                    && ((Page)this.CurrentView).NavigationCacheMode == NavigationCacheMode.Disabled)
//                {
//                    var currentView = this.CurrentView;
//                    var currentViewDisposable = currentView as IDisposable;
//
//                    // if(currentView is BasePage
//                    if (currentViewDisposable != null)
//                    {
//                        currentViewDisposable.Dispose();
//                        currentViewDisposable = null;
//                    } //view model is disposed in the view implementation
//                    currentView = null;
//                    GC.Collect();
//                }
//            }
//            catch { }
//        }
//
//        public void Navigate(string pageKey)
//        {
//            NavigateTo(new Uri(string.Format("/Views/{0}PageView.xaml", pageKey), UriKind.Relative));
//        }
//
//        public void Navigate(string pageKey, object parameter)
//        {
//            DisposePreviousView();
//            var key = Guid.NewGuid().ToString();
//            _parameters.Add(key, parameter);
//            NavigateTo(new Uri(string.Format("/Views/{0}PageView.xaml?x-guid={1}", pageKey, key), UriKind.Relative));
//        }
//
//        private void NavigateTo(Uri pageUri)
//        {
//            Frame.Navigate(pageUri);
//        }
//
//        public void GoBack()
//        {
//            if (Frame.CanGoBack)
//            {
//                Frame.GoBack();
//            }
//        }
//    }
//}
