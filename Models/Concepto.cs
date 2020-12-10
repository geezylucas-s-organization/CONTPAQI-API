namespace CONTPAQ_API.Models
{
    public class Concepto
    {
        public int codigoConcepto { get; set; }
        public string nombreConcepto { get; set; }
        public int noFolio { get; set; }

        public Concepto(int codigoConcepto, string nombreConcepto, int noFolio)
        {
            this.codigoConcepto = codigoConcepto;
            this.nombreConcepto = nombreConcepto;
            this.noFolio = noFolio;
        }
    }
}