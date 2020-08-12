using System;
using System.Collections.Generic;

namespace TodoApi
{
    public class FunctionReturnedValue
    {
        public bool isValid { get; set; }
        public string message { get; set; }
        public List<Producto> productos { get; set; }
        public List<Cliente> clientes { get; set; }
        public List<Concepto> conceptos { get; set; }
        public FunctionReturnedValue(bool isValid, string message)
        {
            this.isValid = isValid;
            this.message = message;
        }

        public FunctionReturnedValue(bool isValid)
        {
            this.isValid = isValid;
            this.message = string.Empty;
        }

        public FunctionReturnedValue(bool isValid, List<Producto> productos)
        {
            this.isValid = isValid;
            this.productos = productos;
        }
        
        
        public FunctionReturnedValue(bool isValid, List<Cliente> clientes)
        {
            this.isValid = isValid;
            this.clientes = clientes;
        }

        public FunctionReturnedValue(bool isValid, List<Concepto> conceptos)
        {
            this.isValid = isValid;
            this.conceptos = conceptos;
        }
        
    }
}