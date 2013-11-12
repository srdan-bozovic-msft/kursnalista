using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursnaLista.Phone.Models;

namespace KursnaListaPhoneApp.ViewModels
{
    public class StavkaKursneListeViewModel
    {
        private readonly StavkaKursneListe _item;

        public StavkaKursneListeViewModel(StavkaKursneListe item)
        {
            _item = item;
        }

        public int SifraValute
        {
            get { return _item.SifraValute; }
        }

        public string NazivZemlje
        {
            get { return _item.NazivZemlje; }
        }

        public string OznakaValute
        {
            get { return _item.OznakaValute; }
        }

        public int VaziZa
        {
            get { return _item.VaziZa; }
        }

        public decimal SrednjiKurs
        {
            get { return _item.SrednjiKurs; }
        }

        public decimal KupovniKurs
        {
            get { return _item.KupovniKurs; }
        }

        public decimal ProdajniKurs
        {
            get { return _item.ProdajniKurs; }
        }
    }
}
