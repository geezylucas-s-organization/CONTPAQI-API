namespace CONTPAQ_API
{
    public class ProductoJSON
    {
        public string cCodigoProducto { get; set; }

        public string cNombreProducto { get; set; }

        public string cDescripcionProducto { get; set; }

        public int cTipoProducto { get; set; } // 1 = Producto, 2 = Paquete, 3 = Servicio

        public string cFechaAltaProducto { get; set; }

        public string cFechaBaja { get; set; }

        public int cStatusProducto { get; set; } // 0 - Baja Lógica, 1 - Alta

        public int cControlExistencia { get; set; }

        public int cMetodoCosteo { get; set; } // 1 = Costo Promedio en Base a Entradas,
                                               // 2 = Costo Promedio en Base a Entradas Almacen,
                                               // 3 = Último costo,
                                               // 4 = UEPS,
                                               // 5 = PEPS,
                                               // 6 = Costo específico,
                                               // 7 = Costo Estandar

        public string cCodigoUnidadBase { get; set; }

        public string cCodigoUnidadNoConvertible { get; set; }

        public float cPrecio1 { get; set; }
        public float cPrecio2 { get; set; }

        public float cPrecio3 { get; set; }

        public float cPrecio4 { get; set; }

        public float cPrecio5 { get; set; }

        public float cPrecio6 { get; set; }

        public float cPrecio7 { get; set; }

        public float cPrecio8 { get; set; }

        public float cPrecio9 { get; set; }

        public float cPrecio10 { get; set; }

        public float cImpuesto1 { get; set; }
        public float cImpuesto2 { get; set; }

        public float cImpuesto3 { get; set; }

        public float cRetencion1 { get; set; }
        public float cRetencion2 { get; set; }

        public string cNombreCaracteristica1 { get; set; }
        public string cNombreCaracteristica2 { get; set; }

        public string cNombreCaracteristica3 { get; set; }

        public string cCodigoValorClasificacion1 { get; set; }
        public string cCodigoValorClasificacion2 { get; set; }

        public string cCodigoValorClasificacion3 { get; set; }

        public string cCodigoValorClasificacion4 { get; set; }

        public string cCodigoValorClasificacion5 { get; set; }

        public string cCodigoValorClasificacion6 { get; set; }

        public string cTextoExtra1 { get; set; }
        public string cTextoExtra2 { get; set; }

        public string cTextoExtra3 { get; set; }
        public string cFechaExtra { get; set; }

        public float cImporteExtra1 { get; set; }
        public float cImporteExtra2 { get; set; }
        public float cImporteExtra3 { get; set; }
        public float cImporteExtra4 { get; set; }
    }
}