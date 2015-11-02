using System.Threading.Tasks;

namespace MSC.Universal.Shared.UI.Contracts.Services
{
    public interface IDialogService
    {
        Task ShowMessageAsync(string message, string caption = "");
        Task<bool> ShowConsentAsync(string message, string caption = "");
    }
}
