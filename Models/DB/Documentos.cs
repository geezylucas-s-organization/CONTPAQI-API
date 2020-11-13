using System;
using System.Collections.Generic;

namespace CONTPAQ_API.Models.DB
{
    public partial class Documentos
    {
        public Documentos()
        {
            Movimientos = new HashSet<Movimientos>();
        }

        public int Documentoid { get; set; }
        public string Descripcion { get; set; }
        public DateTime? UltimaVezFacturada { get; set; }
        public DateTime? ProximaFactura { get; set; }
        public int? PeriodoDias { get; set; }
        public bool Estatus { get; set; }

        public virtual Cabeceras Cabeceras { get; set; }
        public virtual ICollection<Movimientos> Movimientos { get; set; }
    }
}
