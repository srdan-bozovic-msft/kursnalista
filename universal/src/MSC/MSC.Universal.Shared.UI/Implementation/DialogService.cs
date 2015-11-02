using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using MSC.Universal.Shared.UI.Contracts.Services;

namespace MSC.Universal.Shared.UI.Implementation
{
    public class DialogService : IDialogService
    {
        public async Task ShowMessageAsync(string message, string caption)
        {
            var dialog = new MessageDialog(message, caption);
            dialog.Commands.Add(new UICommand("OK"));
            await dialog.ShowAsync();
        }

        public async Task<bool> ShowConsentAsync(string message, string caption)
        {
            var dialog = new MessageDialog(message, caption);
            dialog.Commands.Add(new UICommand("OK"));
            dialog.Commands.Add(new UICommand("Cancel"));
            return (await dialog.ShowAsync()).Label == "OK";
        }
    }
}
