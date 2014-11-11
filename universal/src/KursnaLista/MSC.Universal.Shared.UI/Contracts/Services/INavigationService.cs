using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using MSC.Universal.Shared.UI.Contracts.Views;

namespace MSC.Universal.Shared.UI.Contracts.Services
{
    public interface INavigationService
    {
        Frame Frame { get; set; }
        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in the back navigation history.
        /// </summary>
        /// <remarks>Returns bool. True if there is at least one entry in the back navigation history; otherwise, false.</remarks>
        bool CanGoBack { get; }
        bool CanGoForward { get; }

        T GetParameter<T>(string parameterName);

        /// <summary>
        /// Navigates to the content specified by the uniform resource identifier (URI).
        /// </summary>
        /// <param name="parameter">The URI of the content to navigate to.</param>
        /// <param name="removeBackEntry">Replace page in current navigation stack</param>
        /// <returns>Returns bool. True if the navigation started successfully; otherwise, false.</returns>
        bool NavigateTo<T>(object parameter = null, bool removeBackEntry = false) where T : IPageView;

        void NavigateTo(Uri uri);

        bool NavigateHome();

        /// <summary>
        /// Navigates to the most recent entry in the back navigation history, or throws an exception if no entry exists in back navigation.
        /// </summary>
        void GoBack();
        void GoForward();
    }
}
