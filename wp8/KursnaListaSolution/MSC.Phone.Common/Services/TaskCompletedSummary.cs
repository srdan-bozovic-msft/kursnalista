namespace MSC.Phone.Common.Services
{
    public class TaskCompletedSummary<T>
    {
        public string Task { get; set; }

        public TaskSummaryResult Result { get; set; }

        public T Context { get; set; }
    }
}