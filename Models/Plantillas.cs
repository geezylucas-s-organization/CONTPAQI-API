using System;
using Microsoft.AspNetCore.Components.Web;

namespace CONTPAQ_API.Models
{
    public class Plantillas
    {
        public int id { get; set; }
        public string jsonFactura { get; set; }
        public DateTime ultimaVezFacturada { get; set; }
        public DateTime proximaFactura { get; set; }
        public bool estatus { get; set; }
    }
}