using MSC.Phone.Shared.Contracts.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
namespace KursnaLista.Phone.Contracts.ViewModels
{
    public interface IMainPageViewModel : IPageViewModel
    {
        string Datum { get; set; }
        bool IsDataLoaded { get; }
        ObservableCollection<IStavkaKursneListeViewModel> SrednjiKursItems { get; }
        ObservableCollection<IStavkaKursneListeViewModel> ZaDevizeItems { get; }
        ObservableCollection<IStavkaKursneListeViewModel> ZaEfektivniStraniNovacItems { get; }
    }
}
