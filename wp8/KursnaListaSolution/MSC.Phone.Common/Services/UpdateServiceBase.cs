using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Common.Services
{
    public class UpdateServiceBase
    {
        protected TaskCompletedSummary<T> HandleException<T>(string taskName, Exception xcp)
        {

            if (xcp is WebException)
            {
                var webException = xcp as WebException;
                var summary = ExceptionHandling.GetSummaryFromWebException<T>(taskName, webException);
                return summary;
            }

            if (xcp is UnauthorizedAccessException)
            {
                return new TaskCompletedSummary<T> {Task = taskName, Result = TaskSummaryResult.AccessDenied};
            }

            return new TaskCompletedSummary<T> {Task = taskName, Result = TaskSummaryResult.UnknownError};
        }
    }
}
