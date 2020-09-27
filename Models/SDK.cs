using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CONTPAQ_API
{
    public class SDK
    {
        #region Declaración de constantes

        public class constantes // Declaración de constantes
        {
            public const int kLongFecha = 24;
            public const int kLongSerie = 12;
            public const int kLongCodigo = 31;
            public const int kLongNombre = 61;
            public const int kLongReferencia = 21;
            public const int kLongDescripcion = 61;
            public const int kLongCuenta = 101;
            public const int kLongMensaje = 3001;
            public const int kLongNombreProducto = 256;
            public const int kLongAbreviatura = 4;
            public const int kLongCodValorClasif = 4;
            public const int kLongDenComercial = 51;
            public const int kLongRepLegal = 51;
            public const int kLongTextoExtra = 51;
            public const int kLongRFC = 21;
            public const int kLongCURP = 21;
            public const int kLongDesCorta = 21;
            public const int kLongNumeroExtInt = 7;
            public const int kLongNumeroExpandido = 31;
            public const int kLongCodigoPostal = 7;
            public const int kLongTelefono = 16;
            public const int kLongEmailWeb = 51;
            public const int kLongSelloSat = 176;
            public const int kLonSerieCertSAT = 21;
            public const int kLongFechaHora = 36;
            public const int kLongSelloCFDI = 176;
            public const int kLongCadOrigComplSAT = 501;
            public const int kLongitudUUID = 37;
            public const int kLongitudRegimen = 101;
            public const int kLongitudMoneda = 61;
            public const int kLongitudFolio = 17;
            public const int kLongitudMonto = 31;
            public const int kLogitudLugarExpedicion = 401;
        }

        #endregion

        #region Declaración de estructuras

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tCteProv
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public String cCodigoCliente; //[ kLongCodigo + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongNombre)]
            public String cRazonSocial; //[ kLongNombre + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongFecha)]
            public String cFechaAlta; //[ kLongFecha + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongRFC)]
            public String cRFC; //[ kLongRFC + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCURP)]
            public String cCURP; //[ kLongCURP + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongDenComercial)]
            public String cDenComercial; //[ kLongDenComercial + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongRepLegal)]
            public String cRepLegal; //[ kLongRepLegal + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongNombre)]
            public String cNombreMoneda; //[ kLongNombre + 1 ];

            public int cListaPreciosCliente;
            public double cDescuentoMovto;
            public int cBanVentaCredito; // 0 = No se permite venta a crédito, 1 = Se permite venta a crédito

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public String cCodigoValorClasificacionCliente1; //[ kLongCodValorClasif + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public String cCodigoValorClasificacionCliente2; //[ kLongCodValorClasif + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public String cCodigoValorClasificacionCliente3; //[ kLongCodValorClasif + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public String cCodigoValorClasificacionCliente4; //[ kLongCodValorClasif + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public String cCodigoValorClasificacionCliente5; //[ kLongCodValorClasif + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public String cCodigoValorClasificacionCliente6; //[ kLongCodValorClasif + 1 ];

            public int cTipoCliente; // 1 - Cliente, 2 - Cliente/Proveedor, 3 - Proveedor
            public int cEstatus; // 0. Inactivo, 1. Activo

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongFecha)]
            public String cFechaBaja; //[ kLongFecha + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongFecha)]
            public String cFechaUltimaRevision; //[ kLongFecha + 1 ];

            public double cLimiteCreditoCliente;
            public int cDiasCreditoCliente;
            public int cBanExcederCredito; // 0 = No se permite exceder crédito, 1 = Se permite exceder el crédito
            public double cDescuentoProntoPago;
            public int cDiasProntoPago;
            public double cInteresMoratorio;
            public int cDiaPago;
            public int cDiasRevision;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongDesCorta)]
            public String cMensajeria; //[ kLongDesCorta + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongDescripcion)]
            public String cCuentaMensajeria; //[ kLongDescripcion + 1 ];

            public int cDiasEmbarqueCliente;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public String cCodigoAlmacen; //[ kLongCodigo + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public String cCodigoAgenteVenta; //[ kLongCodigo + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public String cCodigoAgenteCobro; //[ kLongCodigo + 1 ];

            public int cRestriccionAgente;
            public double cImpuesto1;
            public double cImpuesto2;
            public double cImpuesto3;
            public double cRetencionCliente1;
            public double cRetencionCliente2;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public String cCodigoValorClasificacionProveedor1; //[ kLongCodValorClasif + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public String cCodigoValorClasificacionProveedor2; //[ kLongCodValorClasif + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public String cCodigoValorClasificacionProveedor3; //[ kLongCodValorClasif + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public String cCodigoValorClasificacionProveedor4; //[ kLongCodValorClasif + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public String cCodigoValorClasificacionProveedor5; //[ kLongCodValorClasif + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public String cCodigoValorClasificacionProveedor6; //[ kLongCodValorClasif + 1 ];

            public double cLimiteCreditoProveedor;
            public int cDiasCreditoProveedor;
            public int cTiempoEntrega;
            public int cDiasEmbarqueProveedor;
            public double cImpuestoProveedor1;
            public double cImpuestoProveedor2;
            public double cImpuestoProveedor3;
            public double cRetencionProveedor1;
            public double cRetencionProveedor2;

            public int
                cBanInteresMoratorio; // 0 = No se le calculan intereses moratorios al cliente, 1 = Si se le calculan intereses moratorios al cliente.

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongTextoExtra)]
            public String cTextoExtra1; //[ kLongTextoExtra + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongTextoExtra)]
            public String cTextoExtra2; //[ kLongTextoExtra + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongTextoExtra)]
            public String cTextoExtra3; //[ kLongTextoExtra + 1 ];

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongTextoExtra)]
            public String cFechaExtra; //[ kLongFecha + 1 ];

            public double cImporteExtra1;
            public double cImporteExtra2;
            public double cImporteExtra3;
            public double cImporteExtra4;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tProducto
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string cCodigoProducto;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongNombre)]
            public string cNombreProducto;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongNombreProducto)]
            public string cDescripcionProducto;

            public int cTipoProducto; // 1 = Producto, 2 = Paquete, 3 = Servicio

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongFecha)]
            public string cFechaAltaProducto;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongFecha)]
            public string cFechaBaja;

            public int cStatusProducto; // 0 - Baja L�gica, 1 - Alta
            public int cControlExistencia; //

            public int
                cMetodoCosteo; // 1 = Costo Promedio en Base a Entradas, 2 = Costo Promedio en Base a Entradas Almacen, 3 = �ltimo costo, 4 = UEPS, 5 = PEPS, 6 = Costo espec�fico, 7 = Costo Estandar

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string cCodigoUnidadBase;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string cCodigoUnidadNoConvertible;

            public double cPrecio1;
            public double cPrecio2;
            public double cPrecio3;
            public double cPrecio4;
            public double cPrecio5;
            public double cPrecio6;
            public double cPrecio7;
            public double cPrecio8;
            public double cPrecio9;
            public double cPrecio10;
            public double cImpuesto1;
            public double cImpuesto2;
            public double cImpuesto3;
            public double cRetencion1;

            public double cRetencion2;

            // N.D.8386 La estructura debe recibir el nombre de la caracter�stica padre. (ALRH)
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongNombre)]
            public string cNombreCaracteristica1;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongNombre)]
            public string cNombreCaracteristica2;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongNombre)]
            public string cNombreCaracteristica3;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion1;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion2;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion3;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion4;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion5;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion6;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongTextoExtra)]
            public string cTextoExtra1;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongTextoExtra)]
            public string cTextoExtra2;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongTextoExtra)]
            public string cTextoExtra3;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongFecha)]
            public string cFechaExtra;

            public double cImporteExtra1;
            public double cImporteExtra2;
            public double cImporteExtra3;
            public double cImporteExtra4;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tDocumento
        {
            public double aFolio;
            public int aNumMoneda;
            public double aTipoCambio;
            public double aImporte;
            public double aDescuentoDoc1;
            public double aDescuentoDoc2;
            public int aSistemaOrigen;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string aCodConcepto;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongSerie)]
            public string aSerie;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongFecha)]
            public string aFecha;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string aCodigoCteProv;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string aCodigoAgente;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongReferencia)]
            public string aReferencia;

            public int aAfecta;
            public double aGasto1;
            public double aGasto2;
            public double aGasto3;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tMovimiento
        {
            public int aCosecutivo;
            public double aUnidades;
            public double aPrecio;
            public double aCosto;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string aCodProdSer;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string aCodAlmacen;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongReferencia)]
            public string aReferencia;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string aCodClasificacion;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct RegLlaveDoc
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string aCodConcepto;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongSerie)]
            public string aSerie;

            public double folio;
        }

        #endregion

        #region Funciones generales

        [DllImport("KERNEL32")]
        public static extern int SetCurrentDirectory(string pPtrDirActuak);

        [DllImport("MGWServicios.dll")]
        public static extern int fSetNombrePAQ(string aNombrePAQ);

        [DllImport("MGWServicios.dll")]
        public static extern void fTerminaSDK();

        [DllImport("MGWServicios.dll")]
        public static extern void fError(int NumeroError, StringBuilder Mensaje, int Longitud);

        #endregion

        #region Funciones de empresa

        [DllImport("MGWServicios.dll")]
        public static extern int fAbreEmpresa(string aDirEmpresa);

        [DllImport("MGWServicios.dll")]
        public static extern void fCierraEmpresa();

        [DllImport("MGWServicios.dll")]
        public static extern void fInicioSesionSDK(string usuario, string pass);

        [DllImport("MGWServicios.DLL")]
        public static extern int fPosPrimerEmpresa(ref int vaIdEmpresa, StringBuilder aNombreEmpresa,
            StringBuilder aDirectorioEmpresa);

        [DllImport("MGWServicios.DLL")]
        public static extern int fPosSiguienteEmpresa(ref int vaIdEmpresa, StringBuilder aNombreEmpresa,
            StringBuilder aDirectorioEmpresa);

        #endregion

        #region Funciones de documento

        [DllImport("MGWServicios.DLL")]
        public static extern int fSiguienteFolio(string codConcepto, StringBuilder serie,
            ref double folio);

        [DllImport("MGWServicios.DLL")]
        public static extern int fAltaDocumento(ref int idDocumento, ref tDocumento atDocumento);

        [DllImport("MGWServicios.DLL")]
        public static extern int fAltaMovimiento(int idDocumento, ref int idMovimiento,
            ref tMovimiento astMovimiento);

        [DllImport("MGWServicios.DLL")]
        public static extern int fEmitirDocumento(string codConcepto, string serie, double folio,
            string pass, string archivoAdicional);

        [DllImport("MGWServicios.DLL")]
        public static extern int fAltaDocumentoCargoAbono(ref tDocumento atDocumento);

        [DllImport("MGWServicios.DLL")]
        public static extern Int32 fSaldarDocumento(ref RegLlaveDoc factura, ref RegLlaveDoc docPago, double importe,
            int moneda, string fecha);

        [DllImport("MGWServicios.DLL")]
        public static extern Int32 fEntregEnDiscoXML([MarshalAs(UnmanagedType.LPStr)] string aCodConcepto,
            [MarshalAs(UnmanagedType.LPStr)] string aSerie, double aFolio, int aFormato, string aFormatoAmigable);

        [DllImport("MGWServicios.DLL")]
        public static extern int fPosPrimerDocumento();

        [DllImport("MGWServicios.DLL")]
        public static extern int fPosUltimoDocumento();

        [DllImport("MGWServicios.DLL")]
        public static extern int fPosSiguienteDocumento();

        [DllImport("MGWServicios.DLL")]
        public static extern int fPosAnteriorDocumento();

        [DllImport("MGWServicios.DLL")]
        public static extern int fPosBOF();

        [DllImport("MGWServicios.DLL")]
        public static extern int fPosEOF();

        [DllImport("MGWServicios.DLL")]
        public static extern int fLeeDatoDocumento(string aCampo, StringBuilder aValor, int aLen);

        #endregion

        #region Funciones de producto

        [DllImport("MGWServicios.DLL")]
        public static extern int fBuscaProducto(StringBuilder aCodProducto);

        [DllImport("MGWServicios.DLL")]
        public static extern int fPosPrimerProducto();

        [DllImport("MGWServicios.DLL")]
        public static extern int fPosSiguienteProducto();

        [DllImport("MGWServicios.DLL")]
        public static extern int fLeeDatoProducto(string aCampo, StringBuilder aValor, int aLen);

        [DllImport("MGWServicios.DLL")]
        public static extern int fAltaProducto(ref int aIdProducto, ref tProducto astProducto);

        [DllImport("MGWServicios.dll")]
        public static extern int fBuscaProducto(string aCodProducto);

        [DllImport("MGWServicios.dll")]
        public static extern int fEditaProducto();

        [DllImport("MGWServicios.dll")]
        public static extern int fSetDatoProducto(string aCampo, string aValor);

        [DllImport("MGWServicios.dll")]
        public static extern int fGuardaProducto();

        [DllImport("MGWServicios.dll")]
        public static extern int fEliminarProducto(string aCodigoProducto);

        #endregion

        #region Funciones de Cliente/Proveedor

        [DllImport("MGWServicios.dll")]
        public static extern int fAltaCteProv(ref int aIdCliente, ref tCteProv astCliente);

        [DllImport("MGWServicios.DLL")]
        public static extern int fBuscaCteProv(string aCodCteProv);
        [DllImport("MGWServicios.DLL")]
        public static extern int fBuscaIdCteProv(int aIdCteProv);

        [DllImport("MGWServicios.DLL")]
        public static extern int fLeeDatoCteProv(string aCampo, StringBuilder aValr, int aLen);

        [DllImport("MGWServicios.DLL")]
        public static extern int fEditaCteProv();
        [DllImport("MGWServicios.DLL")]

        public static extern int fPosUltimoCteProv();

        [DllImport("MGWServicios.DLL")]
        public static extern int fSetDatoCteProv(string aCampo, string aValor);

        [DllImport("MGWServicios.DLL")]
        public static extern int fGuardaCteProv();

        [DllImport("MGWServicios.DLL")]
        public static extern int fBorraCteProv(string aCodCteProv);

        [DllImport("MGWServicios.DLL")]
        public static extern int fPosPrimerCteProv();

        [DllImport("MGWServicios.DLL")]
        public static extern int fPosSiguienteCteProv();

        #endregion

        #region Funciones de Conceptos

        [DllImport("MGWServicios.DLL")]
        public static extern int fPosPrimerConceptoDocto();

        [DllImport("MGWServicios.DLL")]
        public static extern int fPosSiguienteConceptoDocto();

        [DllImport("MGWServicios.DLL")]
        public static extern int fLeeDatoConceptoDocto(string aCampo, StringBuilder aValor, int aLen);

        [DllImport("MGWServicios.DLL")]
        public static extern int fBuscaConceptoDocto(string aCodConcepto);

        #endregion

        //Manejo de errores

        public static string rError(int iError)
        {
            StringBuilder msj = new StringBuilder(512);
            if (iError != 0)
            {
                fError(iError, msj, 512);
                Console.WriteLine("Número de error: " + iError.ToString() + " Error: " + msj.ToString());
            }

            return msj.ToString();
        }
    }
}