using System;
using System.Collections.Generic;

namespace CONTPAQ_API.Models.DB
{
    public partial class Movimientos
    {
        public int Documentoid { get; set; }
        public int NumeroMovimiento { get; set; }
        public string CodAlmacen { get; set; }
        public string CodProducto { get; set; }
        public double Precio { get; set; }
        public int Unidades { get; set; }

        public virtual Documentos Documento { get; set; }
    }
}
