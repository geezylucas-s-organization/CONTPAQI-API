using System;
using System.Collections.Generic;

namespace CONTPAQ_API.Models.DB
{
    public partial class Cabeceras
    {
        public int Documentoid { get; set; }
        public int NumMoneda { get; set; }
        public string Serie { get; set; }
        public int TipoCambio { get; set; }
        public string CodConcepto { get; set; }
        public string CodigoCteProv { get; set; }

        public virtual Documentos Documento { get; set; }
    }
}
