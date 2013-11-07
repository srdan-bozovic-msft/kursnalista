using System.Net;

namespace MSC.Phone.Common.Services
{
    public class ExceptionHandling
    {
        public static TaskCompletedSummary<T> GetSummaryFromWebException<T>(string taskName, WebException e)
        {
            var webResponse = e.Response as HttpWebResponse;
            if (webResponse != null && webResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                //// "Access denied // check credentials"
                return new TaskCompletedSummary<T> { Task = taskName, Result = TaskSummaryResult.AccessDenied };
            }

            string response = null;

            try
            {
                using (var stream = e.Response.GetResponseStream())
                {
                    var buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    response = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                }
            }
            catch
            {
                
            }

            if (string.IsNullOrEmpty(response))
            {
                //// "Can not connect to server // check conectivity";
                return new TaskCompletedSummary<T> { Task = taskName, Result = TaskSummaryResult.UnreachableServer };
            }

            return new TaskCompletedSummary<T> { Task = taskName, Result = TaskSummaryResult.UnknownError };
        }
    }
}