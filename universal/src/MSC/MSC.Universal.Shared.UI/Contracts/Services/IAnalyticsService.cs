namespace MSC.Universal.Shared.UI.Contracts.Services
{
    public interface IAnalyticsService
    {
        void NotePageVisitedAsync(string pageLabel);
    }
}
