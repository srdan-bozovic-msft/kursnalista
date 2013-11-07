namespace KursnaListaPhoneLib.Model
{
    public class StavkaKursneListe
    {
        public int SifraValute { get; set; }
        public string NazivZemlje { get; set; }
        public string OznakaValute { get; set; }
        public int VaziZa { get; set; }
        public decimal SrednjiKurs { get; set; }
        public decimal KupovniKurs { get; set; }
        public decimal ProdajniKurs { get; set; }
    }
}