using MSC.Phone.Shared.Contracts.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
namespace KursnaLista.Phone.Contracts.ViewModels
{
    public interface IConverterPageViewModel : IStatefullPageViewModel
    {
        Task LoadData(string from, string to);
        bool IsDataCurrent { get; }
        bool IsDataLoaded { get; }
        string Iznos { get; set; }
        ICommand KonvertujCommand { get; set; }
        //bool PinMode { get; }
        decimal Result { get; set; }        
        //Uri SetTileButtonIconUri { get; }
        //string SetTileButtonText { get; }        
        //ICommand SetTileCommand { get; set; }
        int ValutaIzIndex { get; set; }
        ObservableCollection<IValutaViewModel> ValutaIzItems { get; }
        int ValutaUIndex { get; set; }
        ObservableCollection<IValutaViewModel> ValutaUItems { get; }
    }
}
