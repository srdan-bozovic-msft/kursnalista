using System;

namespace MSC.Universal.Shared.Contracts.Services
{
    public interface ITimeService
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
        bool IsBefore(TimeSpan interval, DateTime utcTime);
        bool IsYesterday(DateTime utcTime);
    }
}
