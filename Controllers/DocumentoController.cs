using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentoController : Controller
    {
        [HttpGet]
        public ActionResult Get()
        {
            Conectar();
            FunctionReturnedValue functionReturnedValue = returnProductos();
            if (functionReturnedValue.isValid)
            {
                CreateDocumentoForm respuesta = new CreateDocumentoForm();
                respuesta.productosYServicios = functionReturnedValue.productos;

                functionReturnedValue = returnClientes();
                if (functionReturnedValue.isValid)
                {
                    respuesta.clientesYProveedores = functionReturnedValue.clientes;
                    functionReturnedValue = returnConceptos();
                    if (functionReturnedValue.isValid)
                    {
                        respuesta.conceptos = functionReturnedValue.conceptos;
                        string jsonString;
                        jsonString = JsonSerializer.Serialize(respuesta);
                        return Ok(jsonString);
                    }
                }
                return Ok(functionReturnedValue.message);
            }

            return StatusCode(500, functionReturnedValue.message);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Documento documento)
        {
            //documento = JsonSerializer.Deserialize<Documento>(doc);
            FunctionReturnedValue functionReturnedValue = Conectar();
            if (functionReturnedValue.isValid)
            {
                CreaDocto(documento);
            }
            else
            {
                return StatusCode(500, functionReturnedValue.message);
            }

            //CreaDoctoPago();
            SDK.fCierraEmpresa();
            SDK.fTerminaSDK();
            Console.Write("success");
            return StatusCode(201);
        }

        private FunctionReturnedValue Conectar()
        {
            string rutaBinarios = @"C:\Program Files (x86)\Compac\COMERCIAL";
            string nombrePAQ = "CONTPAQ I COMERCIAL";
            string rutaEmpresa = @"C:\Compac\Empresas\adpruebas_de_timbrado";
            int lError;

            //Paso 1: Pasar la ruta de los binarios, ya que ahí se encuentra la dll a consumir
            SDK.SetCurrentDirectory(rutaBinarios);
            SDK.fInicioSesionSDK("SUPERVISOR", "");

            //Paso 2: Pasar el nombre del sistema con el cual vamos a trabajar
            lError = SDK.fSetNombrePAQ(nombrePAQ);
            if (lError != 0)
            {
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }

            //Paso 3: Indicar la ruta de la empresa a utilzar.
            lError = SDK.fAbreEmpresa(rutaEmpresa);
            if (lError != 0)
            {
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }

            return new FunctionReturnedValue(true);
        }

        private FunctionReturnedValue CreaDocto(Documento documento)
        {
            int lError;
            StringBuilder serie = new StringBuilder("");
            int idDocto = 0;
            int idMovto = 0;
            SDK.tDocumento lDocto = new SDK.tDocumento();
            SDK.tMovimiento lMovimiento = new SDK.tMovimiento();

            //Paso 1: Conseguir el folio del documento a realizar.
            lError = SDK.fSiguienteFolio(documento.cabecera.codConcepto, documento.cabecera.serie,
                ref documento.cabecera.folio);
            if (lError != 0)
            {
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }

            //Cabecera
            lDocto.aCodConcepto = documento.cabecera.codConcepto;
            lDocto.aCodigoCteProv = documento.cabecera.codigoCteProv;
            lDocto.aFecha = documento.cabecera.fecha;
            lDocto.aFolio = documento.cabecera.folio;
            lDocto.aNumMoneda = documento.cabecera.numMoneda;
            lDocto.aSerie = documento.cabecera.serie.ToString();
            lDocto.aTipoCambio = documento.cabecera.tipoCambio;

            //Paso 2: Dar de alta el documento con su cabecera.
            lError = SDK.fAltaDocumento(ref idDocto, ref lDocto);

            if (lError != 0)
            {
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }

            //Paso 3: Se agregan los movimientos al documento.
            foreach (var item in documento.movimientos)
            {
                lMovimiento.aCodAlmacen = item.codAlmacen;
                lMovimiento.aCodProdSer = item.codProducto;
                lMovimiento.aPrecio = item.precio;
                lMovimiento.aUnidades = item.unidades; //Una compra

                //Se da de alta un movimiento.
                lError = SDK.fAltaMovimiento(idDocto, ref idMovto, ref lMovimiento);
                if (lError != 0)
                {
                    return new FunctionReturnedValue(false, SDK.rError(lError));
                }
            }

            //Paso 4: Se timbra el documento
            lError = SDK.fEmitirDocumento(documento.cabecera.codConcepto,
                documento.cabecera.serie.ToString(), documento.cabecera.folio, "12345678a", "");
            if (lError != 0)
            {
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }

            //Paso 5: Se exporta (si se indica) a XML o PDF
            if (documento.especificaciones.exportXML == true)
            {
                lError = SDK.fEntregEnDiscoXML(documento.cabecera.codConcepto,
                    documento.cabecera.serie.ToString(), documento.cabecera.folio, 0, "");
                if (lError != 0)
                {
                    return new FunctionReturnedValue(false, SDK.rError(lError));
                }
            }

            if (documento.especificaciones.exportPDF == true)
            {
                lError = SDK.fEntregEnDiscoXML(documento.cabecera.codConcepto,
                    documento.cabecera.serie.ToString(), documento.cabecera.folio, 1, "");
                if (lError != 0)
                {
                    return new FunctionReturnedValue(false, SDK.rError(lError));
                }
            }

            return new FunctionReturnedValue(true);
        }

        private static void CreaDoctoPago()
        {
            int lError = 0;
            StringBuilder serie = new StringBuilder("");
            double folio = 0;
            string codConcepto = "42017";
            string codCte = "CL001";
            SDK.tDocumento lDocto = new SDK.tDocumento();
            SDK.RegLlaveDoc factura = new SDK.RegLlaveDoc();
            SDK.RegLlaveDoc pago = new SDK.RegLlaveDoc();
            lError = SDK.fSiguienteFolio(codConcepto, serie, ref folio);
            if (lError != 0)
            {
                SDK.rError(lError);
            }

            else
            {
                lDocto.aCodConcepto = codConcepto;
                lDocto.aCodigoCteProv = codCte;
                lDocto.aFecha = DateTime.Today.ToString("MM/dd/yyyy");
                lDocto.aFolio = folio;
                lDocto.aImporte = 464;
                lDocto.aNumMoneda = 1;
                lDocto.aTipoCambio = 1;
                lDocto.aSerie = serie.ToString();

                lError = SDK.fAltaDocumentoCargoAbono(ref lDocto);

                if (lError != 0)
                {
                    SDK.rError(lError);
                }
                else
                {
                    factura.aCodConcepto = "42017";
                    factura.aSerie = "";
                    factura.folio = 2535;

                    pago.aCodConcepto = codConcepto;
                    pago.aSerie = serie.ToString();
                    pago.folio = folio;

                    lError = SDK.fSaldarDocumento(ref factura, ref pago, 464, 1, DateTime.Today.ToString("MM/dd/yyyy"));

                    if (lError != 0)
                    {
                        SDK.rError(lError);
                    }
                    else
                    {
                    }
                }
            }
        }

        private FunctionReturnedValue returnProductos()
        {
            List<Producto> lProductos = new List<Producto>();
            StringBuilder aValor = new StringBuilder();
            string codigoProducto, nombreProducto;
            int lError = 0;

            lError = SDK.fPosPrimerProducto();

            if (lError != 0)
            {
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }

            do
            {
                lError = SDK.fPosSiguienteProducto();
                codigoProducto = string.Empty;
                nombreProducto = string.Empty;

                if (lError != 0)
                {
                    break;
                }

                lError = SDK.fLeeDatoProducto("cCodigoProducto", aValor, 256);

                if (lError != 0)
                {
                    return new FunctionReturnedValue(false, SDK.rError(lError));
                }

                codigoProducto = aValor.ToString();

                lError = SDK.fLeeDatoProducto("cNombreProducto", aValor, 256);

                if (lError != 0)
                {
                    return new FunctionReturnedValue(false, SDK.rError(lError));
                }

                nombreProducto = aValor.ToString();

                lProductos.Add(new Producto(codigoProducto, nombreProducto));
            } while (lError == 0);

            return new FunctionReturnedValue(true, lProductos);
        }

        private FunctionReturnedValue returnClientes()
        {
            List<Cliente> lCliente = new List<Cliente>();
            StringBuilder aValor = new StringBuilder();
            string codigoCliente, razonSocial, rfc, moneda;
            int lError = 0;

            lError = SDK.fPosPrimerCteProv();

            if (lError != 0)
            {
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }

            do
            {
                lError = SDK.fPosSiguienteCteProv();
                codigoCliente = string.Empty;
                razonSocial = string.Empty;
                rfc = string.Empty;
                moneda = string.Empty;

                if (lError != 0)
                {
                    break;
                }

                lError = SDK.fLeeDatoCteProv("cCodigoCliente", aValor, 256);

                if (lError != 0)
                {
                    return new FunctionReturnedValue(false, SDK.rError(lError));
                }

                codigoCliente = aValor.ToString();

                lError = SDK.fLeeDatoCteProv("cRazonSocial", aValor, 256);

                if (lError != 0)
                {
                    return new FunctionReturnedValue(false, SDK.rError(lError));
                }

                razonSocial = aValor.ToString();

                lError = SDK.fLeeDatoCteProv("cRFC",aValor, 256);

                if (lError != 0)
                {
                    return new FunctionReturnedValue(false, SDK.rError(lError));
                }

                rfc = aValor.ToString();

                lError = SDK.fLeeDatoCteProv("cIDMoneda", aValor, 256);

                if (lError != 0)
                {
                    return new FunctionReturnedValue(false, SDK.rError(lError));
                }

                moneda = aValor.ToString();

                lCliente.Add(new Cliente(codigoCliente, razonSocial, rfc, moneda));
            } while (lError == 0);

            return new FunctionReturnedValue(true, lCliente);
        }

        private FunctionReturnedValue returnConceptos()
        {
            List<Concepto> lConcepto = new List<Concepto>();
            StringBuilder aValor = new StringBuilder();
            string nombreConcepto;
            int lError, noFolio;

            lError = SDK.fPosPrimerConceptoDocto();

            if (lError != 0)
            {
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }

            do
            {

                lError = SDK.fLeeDatoConceptoDocto("cNombreConcepto", aValor, 256);

                if (lError != 0)
                {
                    return new FunctionReturnedValue(false, SDK.rError(lError));
                }

                nombreConcepto = aValor.ToString();

                lError = SDK.fLeeDatoConceptoDocto("cNoFolio", aValor, 256);

                if (lError != 0)
                {
                    return new FunctionReturnedValue(false, SDK.rError(lError));
                }

                noFolio = (int)Convert.ToDouble(aValor.ToString());
                noFolio++;

                lConcepto.Add(new Concepto(nombreConcepto, noFolio));
                
                lError = SDK.fPosSiguienteConceptoDocto();
                nombreConcepto = string.Empty;
                noFolio = 0;                
                if (lError != 0)
                {
                    break;
                }
            } while (lError == 0);

            return new FunctionReturnedValue(true, lConcepto);
        }
    }
}