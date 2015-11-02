using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Command;
using MSC.Universal.Shared.Contracts.Services;
using MSC.Universal.Shared.UI.Contracts.Services;
using MSC.Universal.Shared.UI.Contracts.ViewModels;

namespace MSC.Universal.Shared.UI.Implementation
{
    public abstract class SinglePageViewModel : ViewModel, IPageViewModel
    {
        private readonly ITimeService _timeService;
        private readonly IAnalyticsService _analyticsService;
        private readonly bool _enableSharing;

        private string _pageKey;

        /// <summary>
        /// Register this event on the current page to populate the page
        /// with content passed during navigation as well as any saved
        /// state provided when recreating a page from a prior session.
        /// </summary>
        public event LoadStateEventHandler LoadState;
        /// <summary>
        /// Register this event on the current page to preserve
        /// state associated with the current page in case the
        /// application is suspended or the page is discarded from
        /// the navigaqtion cache.
        /// </summary>
        public event SaveStateEventHandler SaveState;

        private readonly INavigationService _navigationService;
        ICommand _goBackCommand;
        ICommand _goForwardCommand;
        protected string _title;


        public ICommand GoBackCommand
        {
            get
            {
                if (_goBackCommand == null)
                {
                    _goBackCommand = new RelayCommand(
                        () => NavigationService.GoBack(),
                        () => NavigationService.CanGoBack);
                }
                return _goBackCommand;
            }
            set
            {
                _goBackCommand = value;
            }
        }

        public ICommand GoForwardCommand
        {
            get
            {
                if (_goForwardCommand == null)
                {
                    _goForwardCommand = new RelayCommand(
                        () => NavigationService.GoForward(),
                        () => NavigationService.CanGoForward);
                }
                return _goForwardCommand;
            }
        }

        public INavigationService NavigationService
        {
            get { return _navigationService; }
        }

        /// <summary>
        /// The class constructor.
        /// </summary>
        /// <param name="navigationService">This parameter provides the ability to navigate between
        /// pages as well as determine if the application has recovered from tombstoning.</param>
        /// <param name="timeService">This parameter provides time</param>
        /// <param name="analyticsService">This parameter provides ability to provide logging services for page</param>
        /// <param name="enableSharing"></param>
        protected SinglePageViewModel(
            INavigationService navigationService,
            ITimeService timeService,
            IAnalyticsService analyticsService,
            bool enableSharing = false)
        {
            _navigationService = navigationService;
            _timeService = timeService;
            _analyticsService = analyticsService;
            _enableSharing = enableSharing;
        }

        public void InitializeSharing()
        {
            var dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested -= DataRequested;
            dataTransferManager.DataRequested += DataRequested;
        }

        private void DataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            FillShareRequest(e.Request.Data);
        }

        public virtual void FillShareRequest(DataPackage dataPackage)
        {
            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.  
        /// This method calls <see cref="LoadState"/>, where all page specific
        /// navigation and process lifetime management logic should be placed.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property provides the group to be displayed.</param>
        public void OnNavigatedTo(NavigationEventArgs e)
        {
            var frameState = SuspensionManager.SessionStateForFrame(NavigationService.Frame);
            _pageKey = "Page-" + NavigationService.Frame.BackStackDepth;

            if(_enableSharing)
                InitializeSharing();

            if (e.NavigationMode == NavigationMode.New)
            {
                // Clear existing state for forward navigation when adding a new page to the
                // navigation stack
                var nextPageKey = _pageKey;
                int nextPageIndex = NavigationService.Frame.BackStackDepth;
                while (frameState.Remove(nextPageKey))
                {
                    nextPageIndex++;
                    nextPageKey = "Page-" + nextPageIndex;
                }

                // Pass the navigation parameter to the new page
                if (LoadState != null)
                {
                    LoadState(this, new LoadStateEventArgs(e.Parameter, null));
                }

                NavigatedTo();
                OnNavigatedToView(e.Parameter, true);
                OnNavigatedForwardToView(e.Parameter);
            }
            else
            {
                // Pass the navigation parameter and preserved page state to the page, using
                // the same strategy for loading suspended state and recreating pages discarded
                // from cache
                if (LoadState != null)
                {
                    LoadState(this, new LoadStateEventArgs(e.Parameter, (Dictionary<String, Object>)frameState[_pageKey]));
                }

                NavigatedTo();
                OnNavigatedToView(e.Parameter, false);
            }
        }

        /// <summary>
        /// Invoked when this page will no longer be displayed in a Frame.
        /// This method calls <see cref="SaveState"/>, where all page specific
        /// navigation and process lifetime management logic should be placed.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property provides the group to be displayed.</param>
        public void OnNavigatedFrom(NavigationEventArgs e)
        {
            var frameState = SuspensionManager.SessionStateForFrame(NavigationService.Frame);
            var pageState = new Dictionary<String, Object>();
            if (SaveState != null)
            {
                SaveState(this, new SaveStateEventArgs(pageState));
            }
            frameState[_pageKey] = pageState;
        }

        public virtual void NavigatedTo()
        {

        }

        public virtual void OnNavigatedToView(dynamic parameter, bool forwardNavigation)
        {

        }

        public virtual void OnNavigatedForwardToView(dynamic parameter)
        {

        }

        public virtual void OnPageDeactivation(NavigatingCancelEventArgs e)
        {
        }

        public void NavigateTo(string url, params object[] args)
        {
            NavigateTo(new Uri(string.Format(url, args), UriKind.Relative));
        }

        public void NavigateTo(Uri uri)
        {
            _navigationService.NavigateTo(uri);
        }

        public ITimeService TimeService { get { return _timeService; } }

        protected IAnalyticsService AnalyticsService
        {
            get { return _analyticsService; }
        }

        public bool IsBefore(TimeSpan interval, DateTime lastSyncDate)
        {
            return _timeService.IsBefore(interval, lastSyncDate);
        }
    }

    /// <summary>
    /// Represents the method that will handle the <see cref="SinglePageViewModel.LoadState"/>event
    /// </summary>
    public delegate void LoadStateEventHandler(object sender, LoadStateEventArgs e);
    /// <summary>
    /// Represents the method that will handle the <see cref="SinglePageViewModel.SaveState"/>event
    /// </summary>
    public delegate void SaveStateEventHandler(object sender, SaveStateEventArgs e);

    /// <summary>
    /// Class used to hold the event data required when a page attempts to load state.
    /// </summary>
    public class LoadStateEventArgs : EventArgs
    {
        /// <summary>
        /// The parameter value passed to <see cref="Frame.Navigate(Type, Object)"/> 
        /// when this page was initially requested.
        /// </summary>
        public Object NavigationParameter { get; private set; }
        /// <summary>
        /// A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.
        /// </summary>
        public Dictionary<string, Object> PageState { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadStateEventArgs"/> class.
        /// </summary>
        /// <param name="navigationParameter">
        /// The parameter value passed to <see cref="Frame.Navigate(Type, Object)"/> 
        /// when this page was initially requested.
        /// </param>
        /// <param name="pageState">
        /// A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.
        /// </param>
        public LoadStateEventArgs(Object navigationParameter, Dictionary<string, Object> pageState)
        {
            NavigationParameter = navigationParameter;
            PageState = pageState;
        }
    }
    /// <summary>
    /// Class used to hold the event data required when a page attempts to save state.
    /// </summary>
    public class SaveStateEventArgs : EventArgs
    {
        /// <summary>
        /// An empty dictionary to be populated with serializable state.
        /// </summary>
        public Dictionary<string, Object> PageState { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveStateEventArgs"/> class.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        public SaveStateEventArgs(Dictionary<string, Object> pageState)
        {
            PageState = pageState;
        }
    }
}
