using System.Collections.Generic;
using System.Text;

namespace CONTPAQ_API
{
    public class Documento
    {
        public Cabecera cabecera { get; set; }
        public List<Movimiento> movimientos { get; set; }
    }

    public class Cabecera
    {
        public string codConcepto { get; set; }
        public string codigoCteProv { get; set; }
        public string fecha { get; set; }
        public double folio = 0;
        public int numMoneda = 1;
        public StringBuilder serie = new StringBuilder("");
        public int tipoCambio = 1;
    }

    public class Movimiento
    {
        public string codAlmacen = "1";
        public string codProducto { get; set; }
        public int precio { get; set; }
        public int unidades { get; set; }
    }

    public class InfoDocumento
    {
        public string codConcepto { get; set; }
        public string nombreConcepto { get; set; }
        public int folio { get; set; }
        public string serie { get; set; }
        
        public string fecha { get; set; }

        //public string codCliente { get; set; }
        public string razonSocialCliente { get; set; }
        public string total { get; set; }
        public string pendiente { get; set; }
    }

    public class ResponseListOfDocuments
    {
        public List<InfoDocumento> data { get; set; }
        public HeaderListOfDocuments header { get; set; }

        public ResponseListOfDocuments(List<InfoDocumento> infoDocumentos, bool isLast, bool isFirst)
        {
            this.data = infoDocumentos;
            this.header = new HeaderListOfDocuments(isLast, isFirst);
        }
    }

    public class HeaderListOfDocuments
    {
        public bool isLast { get; set; }
        public bool isFirst { get; set; }

        public HeaderListOfDocuments(bool isLast, bool isFirst)
        {
            this.isLast = isLast;
            this.isFirst = isFirst;
        }
    }
}