using System.Collections.Generic;
using System.Text;

namespace TodoApi
{
    public class Documento
    {
        public Cabecera cabecera { get; set; }
        public List<Movimiento> movimientos { get; set; }
        public Especificaciones especificaciones { get; set; }
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

    public class Especificaciones
    {
        public bool exportPDF { get; set; }
        public bool exportXML { get; set; }
    }
}