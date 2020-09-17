using System.Runtime.InteropServices;
using System.Text;
using CONTPAQ_API.Controllers;

namespace CONTPAQ_API.Services
{
    public class ClienteServices : ContpaqItem
    {
        public bool create(ClienteJSON cliente)
        {
            int lastProductId = 0;
            SDK.tCteProv lCliente = returnClienteStruct(cliente);

            errorCode = SDK.fAltaCteProv(ref lastProductId, ref lCliente);

            if (errorCode != 0)
            {
                errorMessage = errorCode + ": " + SDK.rError(errorCode);
                createErrorLog();
                return false;
            }

            return true;
        }

        private SDK.tCteProv returnClienteStruct(ClienteJSON cliente)
        {
            return new SDK.tCteProv()
            {
                cCodigoCliente = cliente.cCodigoCliente,
                cRazonSocial = cliente.cRazonSocial,
                cFechaAlta = cliente.cFechaAlta,
                cRFC = cliente.cRFC,
                cCURP = cliente.cCURP,
                cDenComercial = cliente.cDenComercial,
                cRepLegal = cliente.cRepLegal,
                cNombreMoneda = cliente.cNombreMoneda,
                cListaPreciosCliente = cliente.cListaPreciosCliente,
                cDescuentoMovto = cliente.cDescuentoMovto,
                cBanVentaCredito = cliente.cBanVentaCredito,
                cCodigoValorClasificacionCliente1 = cliente.cCodigoValorClasificacionCliente1,
                cCodigoValorClasificacionCliente2 = cliente.cCodigoValorClasificacionCliente2,
                cCodigoValorClasificacionCliente3 = cliente.cCodigoValorClasificacionCliente3,
                cCodigoValorClasificacionCliente4 = cliente.cCodigoValorClasificacionCliente4,
                cCodigoValorClasificacionCliente5 = cliente.cCodigoValorClasificacionCliente5,
                cCodigoValorClasificacionCliente6 = cliente.cCodigoValorClasificacionCliente6,
                cTipoCliente = cliente.cTipoCliente,
                cEstatus = cliente.cEstatus,
                cFechaBaja = cliente.cFechaBaja,
                cFechaUltimaRevision = cliente.cFechaUltimaRevision,
                cLimiteCreditoCliente = cliente.cLimiteCreditoCliente,
                cDiasCreditoCliente = cliente.cDiasCreditoCliente,
                cBanExcederCredito = cliente.cBanExcederCredito,
                cDescuentoProntoPago = cliente.cDescuentoProntoPago,
                cDiasProntoPago = cliente.cDiasProntoPago,
                cInteresMoratorio = cliente.cInteresMoratorio,
                cDiaPago = cliente.cDiaPago,
                cDiasRevision = cliente.cDiasRevision,
                cMensajeria = cliente.cMensajeria,
                cCuentaMensajeria = cliente.cCuentaMensajeria,
                cDiasEmbarqueCliente = cliente.cDiasEmbarqueCliente,
                cCodigoAlmacen = cliente.cCodigoAlmacen,
                cCodigoAgenteVenta = cliente.cCodigoAgenteVenta,
                cCodigoAgenteCobro = cliente.cCodigoAgenteCobro,
                cRestriccionAgente = cliente.cRestriccionAgente,
                cImpuesto1 = cliente.cImpuesto1,
                cImpuesto2 = cliente.cImpuesto2,
                cImpuesto3 = cliente.cImpuesto3,
                cRetencionCliente1 = cliente.cRetencionCliente1,
                cRetencionCliente2 = cliente.cRetencionCliente2,
                cCodigoValorClasificacionProveedor1 = cliente.cCodigoValorClasificacionCliente1,
                cCodigoValorClasificacionProveedor2 = cliente.cCodigoValorClasificacionCliente2,
                cCodigoValorClasificacionProveedor3 = cliente.cCodigoValorClasificacionCliente3,
                cCodigoValorClasificacionProveedor4 = cliente.cCodigoValorClasificacionCliente4,
                cCodigoValorClasificacionProveedor5 = cliente.cCodigoValorClasificacionCliente5,
                cCodigoValorClasificacionProveedor6 = cliente.cCodigoValorClasificacionCliente6,
                cLimiteCreditoProveedor = cliente.cLimiteCreditoProveedor,
                cDiasCreditoProveedor = cliente.cDiasCreditoProveedor,
                cTiempoEntrega = cliente.cTiempoEntrega,
                cDiasEmbarqueProveedor = cliente.cDiasEmbarqueProveedor,
                cImpuestoProveedor1 = cliente.cImpuestoProveedor1,
                cImpuestoProveedor2 = cliente.cImpuestoProveedor2,
                cImpuestoProveedor3 = cliente.cImpuestoProveedor3,
                cRetencionProveedor1 = cliente.cRetencionProveedor1,
                cRetencionProveedor2 = cliente.cRetencionProveedor2,
                cBanInteresMoratorio = cliente.cBanInteresMoratorio,
                cTextoExtra1 = cliente.cTextoExtra1,
                cTextoExtra2 = cliente.cTextoExtra2,
                cTextoExtra3 = cliente.cTextoExtra3,
                cFechaExtra = cliente.cFechaExtra,
                cImporteExtra1 = cliente.cImporteExtra1,
                cImporteExtra2 = cliente.cImporteExtra2,
                cImporteExtra3 = cliente.cImporteExtra3,
                cImporteExtra4 = cliente.cImporteExtra4
            };
        }
    }
}