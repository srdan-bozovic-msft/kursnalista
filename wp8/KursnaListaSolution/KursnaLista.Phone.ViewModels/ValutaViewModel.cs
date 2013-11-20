using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursnaLista.Phone.Models;
using KursnaLista.Phone.Contracts.ViewModels;

namespace KursnaLista.Phone.ViewModels
{
    public class ValutaViewModel : IValutaViewModel
    {
        private readonly StavkaKursneListe _model;

        public ValutaViewModel(StavkaKursneListe model)
        {
            _model = model;
        }
        public string Naziv
        {
            get { return string.Format("{0} ({1})", _model.NazivZemlje, _model.OznakaValute); }
        }

        public string Oznaka
        {
            get { return _model.OznakaValute; }
        }

        public decimal SrednjiKurs
        {
            get { return _model.SrednjiKurs; }
        }
    }
}
