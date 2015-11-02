using System;
using MSC.Universal.Shared.Contracts.Services;

namespace MSC.Universal.Shared.Contracts.Repositories
{
    public class RepositoryResult<T>
    {
        public static implicit operator RepositoryResult<T>(T value)
        {
            return Create(value);
        }

        public static implicit operator T(RepositoryResult<T> result)
        {
            return result.Value;
        }

        public static implicit operator RepositoryResult<T>(ServiceResult<T> serviceResult)
        {
            return Create(
                serviceResult.Value,
                true,
                serviceResult.Successful,
                serviceResult.ErrorCode,
                serviceResult.ErrorMessage,
                serviceResult.ErrorDescription,
                serviceResult.Exception
                );
        }

        public static RepositoryResult<T> Create(T value, bool isCurrent = true, bool successful = true, int errorCode = 0, string errorMessage = null, string errorDescription = null, Exception xcp = null)
        {
            return new RepositoryResult<T>(value, isCurrent, successful, errorCode, errorMessage, errorDescription, xcp);
        }

        public static RepositoryResult<T> CreateError(Exception xcp)
        {
            return new RepositoryResult<T>(xcp);
        }

        public bool IsCurrent { get; protected set; }
        public T Value { get; protected set; }
        public bool Successful { get; protected set; }
        public Exception Exception { get; protected set; }
        public int ErrorCode { get; protected set; }
        public string ErrorMessage { get; protected set; }
        public string ErrorDescription { get; protected set; }

        protected RepositoryResult(T value, bool isCurrent, bool successful, int errorCode, string errorMessage, string errorDescription, Exception xcp)
        {
            Value = value;
            IsCurrent = isCurrent;
            Successful = successful;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            ErrorDescription = errorDescription;
            Exception = xcp;
        }

        protected RepositoryResult(Exception xcp)
        {
            Exception = xcp;
            ErrorCode = xcp.HResult;
            ErrorMessage = xcp.Message;
            Successful = false;
        }
    }
}
