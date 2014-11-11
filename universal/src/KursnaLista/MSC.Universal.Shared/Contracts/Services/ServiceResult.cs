using System;

namespace MSC.Universal.Shared.Contracts.Services
{
    public class ServiceResult<T>
    {
        public static implicit operator ServiceResult<T>(T value)
        {
            return Create(value);
        }

        public static implicit operator T(ServiceResult<T> result)
        {
            return result.Value;
        }

        public static ServiceResult<T> Create(T value, DateTime? valueTime = null, bool successful = true, int errorCode = 0, string errorMessage = null)
        {
            return new ServiceResult<T>(value, valueTime ?? DateTime.Now, successful, errorCode, errorMessage);
        }

        public static ServiceResult<T> CreateError(Exception xcp)
        {
            return new ServiceResult<T>(xcp);
        }

        public T Value { get; protected set; }
        public DateTime ValueTime { get; protected set; }
        public bool Successful { get; protected set; }
        public Exception Exception { get; protected set; }
        public int ErrorCode { get; protected set; }
        public string ErrorMessage { get; protected set; }

        protected ServiceResult(T value, DateTime valueTime, bool successful, int errorCode, string errorMessage)
        {
            Value = value;
            ValueTime = valueTime;
            Successful = successful;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        protected ServiceResult(Exception xcp)
        {
            Exception = xcp;
            ErrorCode = xcp.HResult;
            ErrorMessage = xcp.Message;
            Successful = false;
        }
    }
}
