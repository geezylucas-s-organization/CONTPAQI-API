using System.Collections.Generic;

namespace CONTPAQ_API
{
    public class Producto
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public List<double> precios { get; set; }
        public string claveSAT { get; set; }
        public string tipoProducto { get; set; }
        
        public Producto(string codigo, string nombre,string claveSat, string tipoProducto, List<double> precios)
            : this(codigo, nombre, claveSat, tipoProducto)
        {
            this.precios = precios;
        }

        public Producto(string codigo, string nombre, string claveSat, string tipoProducto)
        {
            this.codigo = codigo;
            this.nombre = nombre;
            this.claveSAT = claveSat;
            this.tipoProducto = tipoProducto;
        }
    }

    public class Cliente
    {
        public string codigo { get; set; }
        public string razonSocial { get; set; }
        public string rfc { get; set; }

        public int idMoneda { get; set; }
        public string moneda { get; set; }
        public string tipoCliente { get; set; }

        public Cliente(string codigo, string razonSocial, string rfc, int idMoneda, string moneda, string tipoCliente)
        {
            this.codigo = codigo;
            this.razonSocial = razonSocial;
            this.rfc = rfc;
            this.idMoneda = idMoneda;
            this.moneda = moneda;
            this.tipoCliente = tipoCliente;
        }
    }
}