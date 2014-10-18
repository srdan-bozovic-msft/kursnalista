using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
//using System.Windows.Navigation;

//using Microsoft.Phone.Shell;
using System.Threading;
using KursnaLista.Phone.Contracts.Repositories;
using KursnaLista.Phone.Models;
using GalaSoft.MvvmLight.Command;
using KursnaLista.Phone.Contracts.ViewModels;
//using MSC.Phone.Shared.Contracts.PhoneServices;
//using System.Windows.Controls;
using MSC.Phone.Shared.UI.Implementation;

namespace KursnaLista.Phone.ViewModels
{
    public class ConverterPageViewModel : PageViewModel, IConverterPageViewModel
    {
        private readonly IKursnaListaRepository _repository;
        //private readonly ITileService _tileService;

        public string ParameterFrom { get; set; }
        public string ParameterTo { get; set; }

        public ConverterPageViewModel(
			IKursnaListaRepository repository 
			//,ITileService tileService
		)
        {
            _repository = repository;
            //_tileService = tileService;
            ValutaIzItems = new ObservableCollection<IValutaViewModel>();
            ValutaUItems = new ObservableCollection<IValutaViewModel>();
            KonvertujCommand = new RelayCommand(
                                            () =>
                                            {
                                                Result = (string.IsNullOrEmpty(Iznos) ? 0 : Convert.ToDecimal(Iznos)) *
                                                    (ValutaIzItems[ValutaIzIndex].SrednjiKurs / ValutaIzItems[ValutaIzIndex].VaziZa) / 
                                                    (ValutaUItems[ValutaUIndex].SrednjiKurs / ValutaUItems[ValutaUIndex].VaziZa);
                                            },
                                               () => ValutaIzIndex != -1 && ValutaUIndex != -1);
            //SetTileCommand = new RelayCommand(SetTile);
            IsDataCurrent = true;
        }

        public ObservableCollection<IValutaViewModel> ValutaIzItems { get; private set; }
        public ObservableCollection<IValutaViewModel> ValutaUItems { get; private set; }

        private int _valutaIzIndex=-1;
        public int ValutaIzIndex
        {
            get { return _valutaIzIndex; }
            set
            {
                Set(ref _valutaIzIndex, value);
                //OnPinModeChanged();
            }
        }


        private int _valutaUIndex=-1;
        public int ValutaUIndex
        {
            get { return _valutaUIndex; }
            set
            {
                Set(ref _valutaUIndex, value);
                //OnPinModeChanged();
            }
        }

        private string _iznos;
        public string Iznos
        {
            get { return _iznos; }
            set 
            {
                Set(ref _iznos, value);
            }
        }

        private decimal _result;
        public decimal Result
        {
            get { return _result; }
            set 
            {
                Set(ref _result, value);
            }
        }

        public ICommand KonvertujCommand { get; set; }
        public ICommand SetTileCommand { get; set; }

        private bool _isDataCurrent;
        public bool IsDataCurrent
        {
            get
            {
                return _isDataCurrent;
            }
            private set
            {
                Set(ref _isDataCurrent, value);
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public async Task InitializeAsync()
        {
            if (!IsDataLoaded)
            {
                await LoadData(ParameterFrom, ParameterTo);
            }
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async Task LoadData(string from, string to)
        {
            var cts = new CancellationTokenSource();

            var result = await _repository.NajnovijaKursnaListaAsync(cts.Token);

            var kursnaListaZaDan = result.Value;
            IsDataCurrent = result.IsCurrent;

            var items = kursnaListaZaDan.SrednjiKurs.OrderBy(s=>s.NazivZemlje).ToList();
            items.Insert(0,
                         new StavkaKursneListe
                             {
                                 NazivZemlje = "Srbija",
                                 OznakaValute = "RSD",
                                 SrednjiKurs = 1.0M,
                                 VaziZa = 1
                             });

            var fromIndex = -1;
            var toIndex = -1;

            var index = 0;
            foreach (var item in items)
            {
                if (item.OznakaValute == from)
                    fromIndex = index;
                if (item.OznakaValute == to)
                    toIndex = index;
                ValutaIzItems.Add(new ValutaViewModel(item));
                ValutaUItems.Add(new ValutaViewModel(item));
                index++;
            }

            ValutaIzIndex = fromIndex;
            ValutaUIndex = toIndex;

            //OnPinModeChanged();
            var relayCommand = KonvertujCommand as RelayCommand;
            if (relayCommand != null) relayCommand.RaiseCanExecuteChanged();

            IsDataLoaded = true;
        }

//        private void OnPinModeChanged()
//        {
//            RaisePropertyChanged("SetTileButtonIconUri");
//            RaisePropertyChanged("SetTileButtonText");
//        }
//
//        public Uri SetTileButtonIconUri
//        {
//            get
//            {
//                return PinMode ? new Uri("/Assets/AppBar/pin.png", UriKind.Relative) : new Uri("/Assets/AppBar/unpin.png", UriKind.Relative);
//            }
//        }
//
//        public string SetTileButtonText
//        {
//            get
//            {
//                return PinMode ? "zakači" : "otkači";
//            }
//        }
//        
//        public bool PinMode
//        {
//            get
//            {
//                if (ValutaIzIndex == -1 || ValutaUIndex == -1)
//                    return true;
//                var from = ValutaIzItems[ValutaIzIndex].Oznaka;
//                var to = ValutaUItems[ValutaUIndex].Oznaka;
//                return !TileExists(from, to);
//            }
//        }

//        private void SetTile()
//        {
//            if(ValutaIzIndex==-1 || ValutaUIndex==-1)
//                return;
//            var from = ValutaIzItems[ValutaIzIndex].Oznaka;
//            var to = ValutaUItems[ValutaUIndex].Oznaka;
//            if (TileExists(from, to))
//            {
//                DeleteTile(from,to);
//            }
//            else
//            {
//                CreateTile(from,to);
//            }
//        }

//        private bool TileExists(string from, string to)
//        {
//            var url = string.Format("/Views/ConverterPageView.xaml?from={0}&to={1}", from, to);
//
//            return _tileService.TileExists(url);
//        }
//
//        private void DeleteTile(string from, string to)
//        {
//            var url = string.Format("/Views/ConverterPageView.xaml?from={0}&to={1}", from, to);
//
//            _tileService.DeleteTile(url);
//
//            OnPinModeChanged();
//        }
//
//        private void CreateTile(string from, string to)
//        {
//            var url = string.Format("/Views/ConverterPageView.xaml?from={0}&to={1}", from, to);
//
//            var tileData = new FlipTileData
//            {
//                Title = string.Format("{0} -> {1}",from, to),
//                BackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileMediumExchange.png", UriKind.Relative),
//                SmallBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileSmallExchange.png", UriKind.Relative),
//            };
//
//            _tileService.CreateTile(url, tileData, true);
//        }

        public async Task LoadStateAsync(IDictionary<string, object> state)
        {
            await LoadData("RSD", "EUR");
            ValutaIzIndex = (int)state["ValutaIzIndex"];
            ValutaUIndex = (int)state["ValutaUIndex"];
            Iznos = (string)state["Iznos"];
            Result = (decimal)state["Result"];
        }

        public async Task SaveStateAsync(IDictionary<string, object> state)
        {
            state["ValutaIzIndex"] = ValutaIzIndex;
            state["ValutaUIndex"] = ValutaUIndex;
            state["Iznos"] = Iznos;
            state["Result"] = Result;
        }
    }
}