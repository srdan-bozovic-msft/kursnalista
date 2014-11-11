using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MSC.Universal.Shared.UI.Contracts.ViewModels;

namespace KursnaLista.Contracts.UI.ViewModels
{
    public interface IConverterPageViewModel : IPageViewModel
    {
        Task LoadData(string from, string to);
        bool IsDataCurrent { get; }
        bool IsDataLoaded { get; }
        string Iznos { get; set; }
        ICommand KonvertujCommand { get; set; }
        bool PinMode { get; }
        decimal Result { get; set; }
        ICommand SetTileCommand { get; set; }
        int ValutaIzIndex { get; set; }
        ObservableCollection<IValutaViewModel> ValutaIzItems { get; }
        int ValutaUIndex { get; set; }
        ObservableCollection<IValutaViewModel> ValutaUItems { get; }
        Task LoadStateAsync(IDictionary<string, object> state);
        Task SaveStateAsync(IDictionary<string, object> state);

    }
}
