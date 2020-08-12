using System.Collections.Generic;

namespace TodoApi
{
    public class CreateDocumentoForm
    {
        public List<Producto> productosYServicios { get; set; }
        public List<Cliente> clientesYProveedores { get; set; }

        public List<Concepto> conceptos { get; set; }
    }

    public class Producto
    {
        public string codigo { get; set; }
        public string nombre { get; set; }

        public Producto(string codigo, string nombre)
        {
            this.codigo = codigo;
            this.nombre = nombre;
        }
    }

    public class Cliente
    {
        public string codigo { get; set; }
        public string razonSocial { get; set; }
        public string rfc { get; set; }

        public string moneda { get; set; }

        public Cliente(string codigo, string razonSocial, string rfc, string moneda)
        {
            this.codigo = codigo;
            this.razonSocial = razonSocial;
            this.rfc = rfc;
            this.moneda = moneda;
        }
    }

    public class Concepto
    {
        public string nombreConcepto { get; set; }
        public int noFolio { get; set; }

        public Concepto(string nombreConcepto, int noFolio)
        {
            this.nombreConcepto = nombreConcepto;
            this.noFolio = noFolio;
        }
    }
}