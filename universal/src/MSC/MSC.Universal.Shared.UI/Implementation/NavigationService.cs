using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MSC.Universal.Shared.Contracts.DI;
using MSC.Universal.Shared.UI.Contracts.Services;
using MSC.Universal.Shared.UI.Contracts.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MSC.Universal.Shared.UI.Implementation
{
    public class NavigationService : INavigationService
    {
        private readonly IWebBrowser _webBrowser;
        private readonly IInstanceFactory _instanceFactory;
        private Frame _frame;

        public Frame Frame
        {
            set
            {
                _frame = value;
                _frame.Navigated += FrameNavigated;
                _frame.Navigating += FrameNavigating;
            }
            get { return _frame; }
        }

        //public Type CurrentSource
        //{
        //    get { return _frame.CurrentSourcePageType; }
        //}

        //public dynamic Parameter { get; private set; }

        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in the back navigation history.
        /// </summary>
        public bool CanGoBack
        {
            get { return _frame != null && _frame.CanGoBack; }
        }

        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in the forward navigation history.
        /// </summary>
        public bool CanGoForward
        {
            get { return _frame != null && _frame.CanGoForward; }
        }

        private JObject Parameter { get; set; }

        public T GetParameter<T>(string parameterName)
        {
            return Parameter[parameterName].Value<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">If frame is null.</exception>
        /// <param name="webBrowser">Web browser service</param>
        /// <param name="instanceFactory"></param>
        public NavigationService(IWebBrowser webBrowser, IInstanceFactory instanceFactory)
        {
            _webBrowser = webBrowser;
            _instanceFactory = instanceFactory;
        }

        /// <summary>
        /// Navigates to the content specified by the uniform resource identifier (URI).
        /// </summary>
        /// <param name="parameter">The parameter to navigate to.</param>
        /// <param name="removeBackEntry">Replaces current entry in history stack.</param>
        /// <returns>Returns bool. True if the navigation started successfully; otherwise, false.</returns>
        public bool NavigateTo<T>(object parameter = null, bool removeBackEntry = false) where T : IPageView
        {
            DisposePreviousView();
            var viewType = _instanceFactory.GetType<T>();
            //Frame.Navigate(viewType, parameter);

            //var result = _frame.Navigate(viewType, parameter);
            var json = "";

            if (parameter != null)
            {
                json = JsonConvert.SerializeObject(parameter);
            }

            var result = _frame.Navigate(viewType, json);
            if (removeBackEntry)
                _frame.BackStack.RemoveAt(_frame.BackStackDepth - 1);
            return result;
        }

        public void NavigateTo(Uri uri)
        {
            _webBrowser.NavigateToAsync(uri);
        }

        public IPageView CurrentView
        {
            get { return _frame.Content as IPageView; }
        }

        private void DisposePreviousView()
        {
            //return;
            try
            {
                if (CurrentView != null && ((Page)CurrentView).NavigationCacheMode ==
                    NavigationCacheMode.Disabled)
                {
                    var currentView = CurrentView;
                    var currentViewDisposable = currentView as IDisposable;
                    ((Page)CurrentView).DataContext = null;

                    // if(currentView is BasePage
                    if (currentViewDisposable != null)
                    {
                        currentViewDisposable.Dispose();
                    } //view model is disposed in the view implementation
                    GC.Collect();
                }
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch { }
        }

        public bool NavigateHome()
        {
            DisposePreviousView();
            if(_frame.BackStackDepth > 0)
            {
                var home = _frame.BackStack[0];

                if (home != null)
                {
                    _frame.Navigate(home.SourcePageType, home.Parameter);

                    _frame.BackStack.Clear();
                    _frame.ForwardStack.Clear();
                }
            }
            else
            {
                _frame.Navigate(_frame.SourcePageType, Parameter);
                _frame.BackStack.Clear();
            }
            return false;
        }

        /// <summary>
        /// Navigates to the most recent entry in the back navigation history, or throws an exception if no entry exists in back navigation.
        /// </summary>
        public void GoBack()
        {
            if (CanGoBack)
            {
                DisposePreviousView();
                _frame.GoBack();
            }
        }

        /// <summary>
        /// Navigates to the most recent entry in the forward navigation history, or throws an exception if no entry exists in forward navigation.
        /// </summary>
        public void GoForward()
        {
            if (CanGoForward)
            {
                _frame.GoForward();
            }
        }

        public event NavigatedEventHandler Navigated;

        void FrameNavigated(object sender, NavigationEventArgs e)
        {
            var handler = Navigated;
            if (e.Parameter != null)
            {
                var json = e.Parameter.ToString();
                Parameter = string.IsNullOrEmpty(json) ? new JObject() : (JObject)JsonConvert.DeserializeObject(json);
            }
            else
            {
                Parameter = new JObject();
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        public event NavigatingCancelEventHandler Navigating;

        void FrameNavigating(object sender, NavigatingCancelEventArgs e)
        {
            var handler = Navigating;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
    }
}