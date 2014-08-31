using System;
using System.Collections.Generic;

namespace KursnaLista.Phone.Models
{
    public class KursnaListaZaDan
    {
        public int Broj { get; set; }
        public DateTime Datum { get; set; }
        public List<StavkaKursneListe> SrednjiKurs { get; set; }
        public List<StavkaKursneListe> ZaDevize { get; set; }
        public List<StavkaKursneListe> ZaEfektivniStraniNovac { get; set; }
    }
}