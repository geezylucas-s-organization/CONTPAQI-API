using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace CONTPAQ_API.Services
{
    public class ProductoServices : ContpaqItem
    {
        public bool create(ProductoJSON producto)
        {
            int lastProductId;
            StringBuilder aValor = new StringBuilder();
            SDK.tProducto lProducto = returnProductoStruct(producto);
            lastProductId = 0;

            errorCode = SDK.fAltaProducto(ref lastProductId, ref lProducto);

            if (errorCode != 0)
            {
                errorMessage = errorCode + ": " + SDK.rError(errorCode);
                createErrorLog();
                return false;
            }

            // TODO: I don't know if your logic is like this 
            errorCode = SDK.fEditaProducto();
            errorCode = SDK.fSetDatoProducto("CCLAVESAT", producto.cClaveSAT);
            errorCode = SDK.fGuardaProducto();

            return true;
        }

        public List<Producto> returnAll()
        {
            throw new System.NotImplementedException();
        }

        private SDK.tProducto returnProductoStruct(ProductoJSON producto)
        {
            return new SDK.tProducto()
            {
                cCodigoProducto = producto.cCodigoProducto,
                cNombreProducto = producto.cNombreProducto,
                cDescripcionProducto = producto.cDescripcionProducto,
                cTipoProducto = producto.cTipoProducto,
                cFechaAltaProducto = producto.cFechaAltaProducto,
                cFechaBaja = producto.cFechaBaja,
                cStatusProducto = producto.cStatusProducto,
                cControlExistencia = producto.cControlExistencia,
                cMetodoCosteo = producto.cMetodoCosteo,
                cCodigoUnidadBase = producto.cCodigoUnidadBase,
                cCodigoUnidadNoConvertible = producto.cCodigoUnidadNoConvertible,
                cPrecio1 = producto.cPrecio1,
                cPrecio2 = producto.cPrecio2,
                cPrecio3 = producto.cPrecio3,
                cPrecio4 = producto.cPrecio4,
                cPrecio5 = producto.cPrecio5,
                cPrecio6 = producto.cPrecio6,
                cPrecio7 = producto.cPrecio7,
                cPrecio8 = producto.cPrecio8,
                cPrecio9 = producto.cPrecio9,
                cPrecio10 = producto.cPrecio10,
                cImpuesto1 = producto.cImpuesto1,
                cImpuesto2 = producto.cImpuesto2,
                cImpuesto3 = producto.cImpuesto3,
                cRetencion1 = producto.cRetencion1,
                cRetencion2 = producto.cRetencion2,
                cNombreCaracteristica1 = producto.cNombreCaracteristica1,
                cNombreCaracteristica2 = producto.cNombreCaracteristica2,
                cNombreCaracteristica3 = producto.cNombreCaracteristica3,
                cCodigoValorClasificacion1 = producto.cCodigoValorClasificacion1,
                cCodigoValorClasificacion2 = producto.cCodigoValorClasificacion2,
                cCodigoValorClasificacion3 = producto.cCodigoValorClasificacion3,
                cCodigoValorClasificacion4 = producto.cCodigoValorClasificacion4,
                cCodigoValorClasificacion5 = producto.cCodigoValorClasificacion5,
                cCodigoValorClasificacion6 = producto.cCodigoValorClasificacion6,
                cTextoExtra1 = producto.cTextoExtra1,
                cTextoExtra2 = producto.cTextoExtra2,
                cTextoExtra3 = producto.cTextoExtra3,
                cImporteExtra1 = producto.cImporteExtra1,
                cImporteExtra2 = producto.cImporteExtra2,
                cImporteExtra3 = producto.cImporteExtra3,
                cImporteExtra4 = producto.cImporteExtra4
            };
        }
    }
}