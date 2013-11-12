using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Navigation;
using KursnaListaPhoneApp.Resources;
using MSC.Phone.Common.ViewModels;
using System.Threading;
using KursnaLista.Phone.Contracts.Repositories;

namespace KursnaListaPhoneApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IKursnaListaRepository _repository;

        public MainViewModel(IKursnaListaRepository repository)
        {
            _repository = repository;
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
            CancellationTokenSource cts = new CancellationTokenSource();

            var kursnaListaZaDan = await _repository.NajnovijaKursnaListaAsync(cts.Token);

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