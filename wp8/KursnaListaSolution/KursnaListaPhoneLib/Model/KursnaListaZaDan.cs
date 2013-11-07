using System;
using System.Collections.Generic;

namespace KursnaListaPhoneLib.Model
{
    public class KursnaListaZaDan
    {
        //public int ID { get; set; }
        public int Broj { get; set; }
        public DateTime Datum { get; set; }
        public List<StavkaKursneListe> SrednjiKurs { get; set; }
        public List<StavkaKursneListe> ZaDevize { get; set; }
        public List<StavkaKursneListe> ZaEfektivniStraniNovac { get; set; }
    }
}