using System.Collections.ObjectModel;
using System.Windows.Input;
using MSC.Universal.Shared.UI.Contracts.ViewModels;

namespace KursnaLista.Contracts.UI.ViewModels
{
    public interface IMainPageViewModel : IPageViewModel
    {
        string Datum { get; set; }
        bool IsDataCurrent { get; }
        bool IsDataLoaded { get; }
        ObservableCollection<IStavkaKursneListeViewModel> SrednjiKursItems { get; }
        ObservableCollection<IStavkaKursneListeViewModel> ZaDevizeItems { get; }
        ObservableCollection<IStavkaKursneListeViewModel> ZaEfektivniStraniNovacItems { get; }
        ICommand GoToConverterCommand { get; set; }
    }
}
