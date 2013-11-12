using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursnaLista.Phone.Models;

namespace KursnaListaPhoneApp.ViewModels
{
    public class ValutaViewModel
    {
        private readonly StavkaKursneListe _item;
        public StavkaKursneListe Model
        {
            get { return _item; }
        }

        public ValutaViewModel(StavkaKursneListe item)
        {
            _item = item;
        }

        public string Naziv
        {
            get { return string.Format("{0} ({1})", _item.NazivZemlje, _item.OznakaValute); }
        }
    }
}
