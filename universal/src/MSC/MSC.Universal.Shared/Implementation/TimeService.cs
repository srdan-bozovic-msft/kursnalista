using System;
using MSC.Universal.Shared.Contracts.Services;

namespace MSC.Universal.Shared.Implementation
{
    public class TimeService : ITimeService
    {
        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        public DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }

        public bool IsBefore(TimeSpan interval, DateTime utcTime)
        {
            return UtcNow - utcTime > interval;
        }

        public bool IsYesterday(DateTime utcTime)
        {
            return utcTime.Date == DateTime.UtcNow.AddDays(-1).Date;
        }
    }
}
