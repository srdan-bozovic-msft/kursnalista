using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

using Microsoft.Phone.Shell;
using System.Threading;
using KursnaLista.Phone.Contracts.Repositories;
using KursnaLista.Phone.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KursnaLista.Phone.Contracts.ViewModels;

namespace KursnaLista.Phone.ViewModels
{
    public class ConverterPageViewModel : ViewModelBase, IConverterPageViewModel
    {
        private readonly IKursnaListaRepository _repository;

        public ConverterPageViewModel(IKursnaListaRepository repository)
        {
            _repository = repository;
            ValutaIzItems = new ObservableCollection<IValutaViewModel>();
            ValutaUItems = new ObservableCollection<IValutaViewModel>();
            KonvertujCommand = new RelayCommand(
                                            () =>
                                            {
                                                Result = (string.IsNullOrEmpty(Iznos) ? 0 : Convert.ToDecimal(Iznos)) * ValutaIzItems[ValutaIzIndex].SrednjiKurs / ValutaUItems[ValutaUIndex].SrednjiKurs;
                                            },
                                               () => ValutaIzIndex != -1 && ValutaUIndex != -1);
            SetTileCommand = new RelayCommand(() => SetTile());
        }

        public ObservableCollection<IValutaViewModel> ValutaIzItems { get; private set; }
        public ObservableCollection<IValutaViewModel> ValutaUItems { get; private set; }

        private int _valutaIzIndex=-1;
        public int ValutaIzIndex
        {
            get { return _valutaIzIndex; }
            set
            {
                _valutaIzIndex = value;
                RaisePropertyChanged("ValutaIzIndex");
                OnPinModeChanged();
            }
        }


        private int _valutaUIndex=-1;
        public int ValutaUIndex
        {
            get { return _valutaUIndex; }
            set
            {
                _valutaUIndex = value;
                RaisePropertyChanged("ValutaUIndex");
                OnPinModeChanged();
            }
        }

        private string _iznos;
        public string Iznos
        {
            get { return _iznos; }
            set 
            {
                _iznos = value;
                RaisePropertyChanged("Iznos");
            }
        }

        private decimal _result;
        public decimal Result
        {
            get { return _result; }
            set 
            {
                _result = value;
                RaisePropertyChanged("Result");
            }
        }

        public RelayCommand KonvertujCommand { get; set; }
        public RelayCommand SetTileCommand { get; set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public async Task InitializeAsync(dynamic parameter)
        {
            if (!IsDataLoaded)
            {
                await LoadData(parameter.From, parameter.To);
            }
        }
        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        protected async Task LoadData(string from, string to)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            var kursnaListaZaDan = await _repository.NajnovijaKursnaListaAsync(cts.Token);
            var items = kursnaListaZaDan.SrednjiKurs.Where(k => k.NazivZemlje != ""
                                                                &&
                                                                kursnaListaZaDan.ZaDevize.Any(
                                                                    d => d.NazivZemlje == k.NazivZemlje))
                                        .ToList();
            items.Insert(0,
                         new StavkaKursneListe()
                             {
                                 NazivZemlje = "Srbija",
                                 OznakaValute = "RSD",
                                 SrednjiKurs = 1.0M
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

            OnPinModeChanged();

            KonvertujCommand.RaiseCanExecuteChanged();

            this.IsDataLoaded = true;
            return;
        }

        public event EventHandler PinModeChanged;

        private void OnPinModeChanged()
        {
            if (PinModeChanged != null && ValutaIzIndex !=-1 && ValutaUIndex != -1)
                PinModeChanged(this, EventArgs.Empty);
        }
        
        public bool PinMode
        {
            get
            {
                var from = ValutaIzItems[ValutaIzIndex].Oznaka;
                var to = ValutaUItems[ValutaUIndex].Oznaka;
                return !TileExists(from, to);
            }
        }

        private void SetTile()
        {
            if(ValutaIzIndex==-1 || ValutaUIndex==-1)
                return;
            var from = ValutaIzItems[ValutaIzIndex].Oznaka;
            var to = ValutaUItems[ValutaUIndex].Oznaka;
            if (TileExists(from, to))
            {
                DeleteTile(from,to);
            }
            else
            {
                CreateTile(from,to);
            }
        }

        private bool TileExists(string from, string to)
        {
            var url = string.Format("/Views/ConverterPage.xaml?from={0}&to={1}", from, to);

            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(o => o.NavigationUri.ToString().Contains(url));
            return tile == null ? false : true;
        }

        private void DeleteTile(string from, string to)
        {
            var url = string.Format("/Views/ConverterPage.xaml?from={0}&to={1}", from, to);

            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(o => o.NavigationUri.ToString().Contains(url));
            if (tile == null) return;

            tile.Delete();

            OnPinModeChanged();
        }

        private void CreateTile(string from, string to)
        {
            var url = string.Format("/Views/ConverterPage.xaml?from={0}&to={1}", from, to);

            var tileData = new FlipTileData
            {
                Title = string.Format("{0} -> {1}",from, to),
                BackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileMediumExchange.png", UriKind.Relative),
                SmallBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileSmallExchange.png", UriKind.Relative),
            };

            ShellTile.Create(new Uri(url, UriKind.Relative), tileData, true);
        }
    }
}