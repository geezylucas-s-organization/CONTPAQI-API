using System.Collections.Generic;

namespace CONTPAQ_API
{
    public class CreateDocumentoView
    {
        public List<Producto> productosYServicios { get; set; }
        public List<Cliente> clientesYProveedores { get; set; }

        public List<Concepto> conceptos { get; set; }
    }

    public class Producto
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public List<double> precios { get; set; }

        public Producto(string codigo, string nombre, List<double> precios)
        {
            this.codigo = codigo;
            this.nombre = nombre;
            this.precios = precios;
        }

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

        public int moneda { get; set; }

        public Cliente(string codigo, string razonSocial, string rfc, int moneda)
        {
            this.codigo = codigo;
            this.razonSocial = razonSocial;
            this.rfc = rfc;
            this.moneda = moneda;
        }
    }

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