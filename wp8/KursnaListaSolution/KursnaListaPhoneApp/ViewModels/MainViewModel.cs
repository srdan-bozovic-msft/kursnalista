using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Navigation;
using KursnaListaPhoneApp.Resources;
using KursnaListaPhoneLib.Model;
using KursnaListaPhoneLib.Services;
using KursnaListaPhoneLib.Storage;
using MSC.Phone.Common.ViewModels;

namespace KursnaListaPhoneApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IKursnaListaClient _client;
        private readonly IKursnaListaStore _store;

        public MainViewModel(IKursnaListaClient client, IKursnaListaStore store)
        {
            _client = client;
            _store = store;
            this.ZaDevizeItems = new ObservableCollection<StavkaKursneListeViewModel>();
            this.ZaEfektivniStraniNovacItems = new ObservableCollection<StavkaKursneListeViewModel>();
            this.SrednjiKursItems = new ObservableCollection<StavkaKursneListeViewModel>();
        }

        public ObservableCollection<StavkaKursneListeViewModel> ZaDevizeItems { get; private set; }
        public ObservableCollection<StavkaKursneListeViewModel> ZaEfektivniStraniNovacItems { get; private set; }
        public ObservableCollection<StavkaKursneListeViewModel> SrednjiKursItems { get; private set; }

        private string _datum;

        public string Datum
        {
            get { return _datum; }
            set { SetProperty(ref _datum, value); }
        }

        public bool IsDataLoaded { get; private set; }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async Task LoadData()
        {
            for (int i = 0; i < 100; i++)
            {

                if (await _store.KursnaListaZaDaneNeedsUpdate())
                {
                    var result = await _client.UpdateKursnaListaZaDane(30);
                }

                var kursnaListaZaDane = await _store.GetKursnaListaZaDane();

                if (kursnaListaZaDane.Count > 0)
                {
                    var kursnaListaZaDan = kursnaListaZaDane.Last();

                    Datum = kursnaListaZaDan.Datum.ToShortDateString();

                    foreach (var item in kursnaListaZaDan.ZaDevize)
                    {
                        if (!string.IsNullOrEmpty(item.NazivZemlje))
                            ZaDevizeItems.Add(new StavkaKursneListeViewModel(item));
                    }

                    foreach (var item in kursnaListaZaDan.ZaEfektivniStraniNovac)
                    {
                        if (!string.IsNullOrEmpty(item.NazivZemlje))
                            ZaEfektivniStraniNovacItems.Add(new StavkaKursneListeViewModel(item));
                    }

                    foreach (var item in kursnaListaZaDan.SrednjiKurs)
                    {
                        if (!string.IsNullOrEmpty(item.NazivZemlje))
                            SrednjiKursItems.Add(new StavkaKursneListeViewModel(item));
                    }

                    this.IsDataLoaded = true;
                    return;
                }
            }
        }
    }
}