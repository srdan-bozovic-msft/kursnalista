using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using KursnaListaPhoneApp.Resources;
using KursnaListaPhoneLib.Model;
using KursnaListaPhoneLib.Services;
using KursnaListaPhoneLib.Storage;
using MSC.Phone.Common.ViewModels;
using Microsoft.Phone.Shell;

namespace KursnaListaPhoneApp.ViewModels
{
    public class ConverterViewModel : ViewModelBase
    {
        private readonly IKursnaListaClient _client;
        private readonly IKursnaListaStore _store;

        public ConverterViewModel(IKursnaListaClient client, IKursnaListaStore store)
        {
            _client = client;
            _store = store;
            ValutaIzItems = new ObservableCollection<ValutaViewModel>();
            ValutaUItems = new ObservableCollection<ValutaViewModel>();
            KonvertujCommand = new Command(o => ValutaIzIndex != -1 && ValutaUIndex != -1,
                                            o =>
                                               {
                                                   Result = (string.IsNullOrEmpty(Iznos) ? 0 : Convert.ToDecimal(Iznos)) * ValutaIzItems[ValutaIzIndex].Model.SrednjiKurs / ValutaUItems[ValutaUIndex].Model.SrednjiKurs;
                                               });
            SetTileCommand = new Command(o => SetTile());
        }

        public ObservableCollection<ValutaViewModel> ValutaIzItems { get; private set; }
        public ObservableCollection<ValutaViewModel> ValutaUItems { get; private set; }

        private int _valutaIzIndex=-1;
        public int ValutaIzIndex
        {
            get { return _valutaIzIndex; }
            set
            {
                SetProperty(ref _valutaIzIndex, value);
                OnPinModeChanged();
            }
        }


        private int _valutaUIndex=-1;
        public int ValutaUIndex
        {
            get { return _valutaUIndex; }
            set
            {
                SetProperty(ref _valutaUIndex, value);
                OnPinModeChanged();
            }
        }

        private string _iznos;
        public string Iznos
        {
            get { return _iznos; }
            set { SetProperty(ref _iznos, value); }
        }

        private decimal _result;
        public decimal Result
        {
            get { return _result; }
            set { SetProperty(ref _result, value); }
        }

        public Command KonvertujCommand { get; set; }
        public Command SetTileCommand { get; set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async Task LoadData(string from, string to)
        {
            for (int i = 0; i < 100; i++)
            {
                if (await _store.KursnaListaZaDaneNeedsUpdate())
                {
                    await _client.UpdateKursnaListaZaDane(30);
                }
                var kursnaListaZaDane = await _store.GetKursnaListaZaDane();

                if (kursnaListaZaDane.Count > 0)
                {
                    var kursnaListaZaDan = kursnaListaZaDane.Last();
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

                    KonvertujCommand.ExecuteChanged();

                    this.IsDataLoaded = true;
                    return;
                }
            }
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
                var from = ValutaIzItems[ValutaIzIndex].Model.OznakaValute;
                var to = ValutaUItems[ValutaUIndex].Model.OznakaValute;
                return !TileExists(from, to);
            }
        }

        private void SetTile()
        {
            if(ValutaIzIndex==-1 || ValutaUIndex==-1)
                return;
            var from = ValutaIzItems[ValutaIzIndex].Model.OznakaValute;
            var to = ValutaUItems[ValutaUIndex].Model.OznakaValute;
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