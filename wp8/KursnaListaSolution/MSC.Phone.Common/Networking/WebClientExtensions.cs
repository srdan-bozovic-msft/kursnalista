using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace System.Threading.Tasks
{
    /// <summary>Extension methods for working with WebClient asynchronously.</summary>
    public static class WebClientExtensions
    {
        /// <summary>Downloads the resource with the specified URI as a string, asynchronously.</summary>
        /// <param name="webClient">The WebClient.</param>
        /// <param name="address">The URI from which to download data.</param>
        /// <returns>A Task that contains the downloaded string.</returns>
        public static Task<string> DownloadStringTask(this WebClient webClient, string address)
        {
            return DownloadStringTask(webClient, new Uri(address));
        }

        /// <summary>Downloads the resource with the specified URI as a string, asynchronously.</summary>
        /// <param name="webClient">The WebClient.</param>
        /// <param name="address">The URI from which to download data.</param>
        /// <returns>A Task that contains the downloaded string.</returns>
        public static Task<string> DownloadStringTask(this WebClient webClient, Uri address)
        {
            // Create the task to be returned
            var tcs = new TaskCompletionSource<string>(address);
            
            // Setup the callback event handler
            DownloadStringCompletedEventHandler handler = null;
            handler = (sender, e) => EAPCommon.HandleCompletion(tcs, e, () => e.Result, () => webClient.DownloadStringCompleted -= handler);
            webClient.DownloadStringCompleted += handler;

            // Start the async work
            try
            {
                webClient.DownloadStringAsync(address, tcs);
            }
            catch(Exception exc)
            {
                // If something goes wrong kicking off the async work,
                // unregister the callback and cancel the created task
                webClient.DownloadStringCompleted -= handler;
                tcs.TrySetException(exc);
            }

            // Return the task that represents the async operation
            return tcs.Task;
        }

        /// <summary>Opens a readable stream for the data downloaded from a resource, asynchronously.</summary>
        /// <param name="webClient">The WebClient.</param>
        /// <param name="address">The URI for which the stream should be opened.</param>
        /// <returns>A Task that contains the opened stream.</returns>
        public static Task<Stream> OpenReadTask(this WebClient webClient, string address)
        {
            return OpenReadTask(webClient, new Uri(address));
        }

        /// <summary>Opens a readable stream for the data downloaded from a resource, asynchronously.</summary>
        /// <param name="webClient">The WebClient.</param>
        /// <param name="address">The URI for which the stream should be opened.</param>
        /// <returns>A Task that contains the opened stream.</returns>
        public static Task<Stream> OpenReadTask(this WebClient webClient, Uri address)
        {
            // Create the task to be returned
            var tcs = new TaskCompletionSource<Stream>(address);

            // Setup the callback event handler
            OpenReadCompletedEventHandler handler = null;
            handler = (sender, e) => EAPCommon.HandleCompletion(tcs, e, () => e.Result, () => webClient.OpenReadCompleted -= handler);
            webClient.OpenReadCompleted += handler;

            // Start the async work
            try
            {
                webClient.OpenReadAsync(address, tcs);
            }
            catch(Exception exc)
            {
                // If something goes wrong kicking off the async work,
                // unregister the callback and cancel the created task
                webClient.OpenReadCompleted -= handler;
                tcs.TrySetException(exc);
            }

            // Return the task that represents the async operation
            return tcs.Task;
        }

        /// <summary>Opens a writeable stream for uploading data to a resource, asynchronously.</summary>
        /// <param name="webClient">The WebClient.</param>
        /// <param name="address">The URI for which the stream should be opened.</param>
        /// <param name="method">The HTTP method that should be used to open the stream.</param>
        /// <returns>A Task that contains the opened stream.</returns>
        public static Task<Stream> OpenWriteTask(this WebClient webClient, string address, string method)
        {
            return OpenWriteTask(webClient, new Uri(address), method);
        }

        /// <summary>Opens a writeable stream for uploading data to a resource, asynchronously.</summary>
        /// <param name="webClient">The WebClient.</param>
        /// <param name="address">The URI for which the stream should be opened.</param>
        /// <param name="method">The HTTP method that should be used to open the stream.</param>
        /// <returns>A Task that contains the opened stream.</returns>
        public static Task<Stream> OpenWriteTask(this WebClient webClient, Uri address, string method)
        {
            // Create the task to be returned
            var tcs = new TaskCompletionSource<Stream>(address);

            // Setup the callback event handler
            OpenWriteCompletedEventHandler handler = null;
            handler = (sender, e) => EAPCommon.HandleCompletion(tcs, e, () => e.Result, () => webClient.OpenWriteCompleted -= handler);
            webClient.OpenWriteCompleted += handler;

            // Start the async work
            try
            {
                webClient.OpenWriteAsync(address, method, tcs);
            }
            catch(Exception exc)
            {
                // If something goes wrong kicking off the async work,
                // unregister the callback and cancel the created task
                webClient.OpenWriteCompleted -= handler;
                tcs.TrySetException(exc);
            }
            
            // Return the task that represents the async operation
            return tcs.Task;
        }

     

        /// <summary>Uploads data in a string to the specified resource, asynchronously.</summary>
        /// <param name="webClient">The WebClient.</param>
        /// <param name="address">The URI to which the data should be uploaded.</param>
        /// <param name="method">The HTTP method that should be used to upload the data.</param>
        /// <param name="data">The data to upload.</param>
        /// <returns>A Task containing the data in the response from the upload.</returns>
        public static Task<string> UploadStringTask(this WebClient webClient, string address, string method, string data)
        {
            return UploadStringTask(webClient, new Uri(address), method, data);
        }

        /// <summary>Uploads data in a string to the specified resource, asynchronously.</summary>
        /// <param name="webClient">The WebClient.</param>
        /// <param name="address">The URI to which the data should be uploaded.</param>
        /// <param name="method">The HTTP method that should be used to upload the data.</param>
        /// <param name="data">The data to upload.</param>
        /// <returns>A Task containing the data in the response from the upload.</returns>
        public static Task<string> UploadStringTask(this WebClient webClient, Uri address, string method, string data)
        {
            // Create the task to be returned
            var tcs = new TaskCompletionSource<string>(address);

            // Setup the callback event handler
            UploadStringCompletedEventHandler handler = null;
            handler = (sender, e) => EAPCommon.HandleCompletion(tcs, e, () => e.Result, ()=> webClient.UploadStringCompleted -= handler);
            webClient.UploadStringCompleted += handler;

            // Start the async work
            try
            {
                webClient.UploadStringAsync(address, method, data, tcs);
            }
            catch(Exception exc)
            {
                // If something goes wrong kicking off the async work,
                // unregister the callback and cancel the created task
                webClient.UploadStringCompleted -= handler;
                tcs.TrySetException(exc);
            }

            // Return the task that represents the async operation
            return tcs.Task;
        }
    }
}