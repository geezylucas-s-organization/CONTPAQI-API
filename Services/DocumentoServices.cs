using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using CONTPAQ_API.Controllers;
using CONTPAQ_API.Models;
using CONTPAQ_API.Models.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CONTPAQ_API.Services
{
    public class DocumentoServices : ContpaqItem
    {
        public bool createDocumento(Documento documento)
        {
            if (documento.timbrar)
            {
                StringBuilder serie = new StringBuilder("");
                int idDocto = 0;
                int idMovto = 0;
                SDK.tDocumento lDocto = new SDK.tDocumento();
                SDK.tMovimiento lMovimiento = new SDK.tMovimiento();

                //Paso 1: Conseguir el folio del documento a realizar.
                errorCode = SDK.fSiguienteFolio(documento.cabecera.codConcepto, documento.cabecera.serie,
                    ref documento.cabecera.folio);
                if (errorCode != 0)
                {
                    errorMessage = SDK.rError(errorCode);
                    return false;
                }

                //Cabecera
                lDocto.aCodConcepto = documento.cabecera.codConcepto;
                lDocto.aCodigoCteProv = documento.cabecera.codigoCteProv;
                lDocto.aFecha = DateTime.Now.ToString("MM/dd/yyyy");
                lDocto.aFolio = documento.cabecera.folio;
                lDocto.aNumMoneda = documento.cabecera.numMoneda;
                lDocto.aSerie = documento.cabecera.serie.ToString();
                lDocto.aTipoCambio = documento.cabecera.tipoCambio;


                //Paso 2: Dar de alta el documento con su cabecera.
                errorCode = SDK.fAltaDocumento(ref idDocto, ref lDocto);

                if (errorCode != 0)
                {
                    errorMessage = SDK.rError(errorCode);
                    return false;
                }

                //Paso 3: Se agregan los movimientos al documento.
                foreach (var item in documento.movimientos)
                {
                    lMovimiento.aCodAlmacen = item.codAlmacen;
                    lMovimiento.aCodProdSer = item.codProducto;
                    lMovimiento.aPrecio = item.precio;
                    lMovimiento.aUnidades = item.unidades; //Una compra

                    //Se da de alta un movimiento.
                    errorCode = SDK.fAltaMovimiento(idDocto, ref idMovto, ref lMovimiento);
                    if (errorCode != 0)
                    {
                        errorMessage = SDK.rError(errorCode);
                        return false;
                    }
                }

                //Paso 4: Se timbra el documento
                errorCode = SDK.fEmitirDocumento(documento.cabecera.codConcepto,
                    documento.cabecera.serie.ToString(), documento.cabecera.folio, "12345678a", "");
                if (errorCode != 0)
                {
                    errorMessage = SDK.rError(errorCode);
                    return false;
                }
                //Paso 5: Se exporta a XML o PDF

                errorCode = SDK.fEntregEnDiscoXML(documento.cabecera.codConcepto,
                    documento.cabecera.serie.ToString(), documento.cabecera.folio, 0, "");
                if (errorCode != 0)
                {
                    errorMessage = SDK.rError(errorCode);
                    return false;
                }

                errorCode = SDK.fEntregEnDiscoXML(documento.cabecera.codConcepto,
                    documento.cabecera.serie.ToString(), documento.cabecera.folio, 1, "");
                if (errorCode != 0)
                {
                    errorMessage = SDK.rError(errorCode);
                    return false;
                }
            }

            if (documento.guardarPlantilla)
            {
                try
                {
                    PlantillasServices.addPlantilla(documento);
                }
                catch (Exception e)
                {
                    errorMessage = e.Message;
                    return false;
                }
            }

            if (documento.docEnPlantiila != null)
            {
                PlantillasServices.updatePlantilla(documento);
            }
            
            return true;
        }

        public static void CreaDoctoPago()
        {
            int lError = 0;
            StringBuilder serie = new StringBuilder("");
            double folio = 0;
            string codConcepto = "42017"; //Conceptos de Pago.
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

        public List<Producto> returnProductos()
        {
            List<Producto> lProductos = new List<Producto>();
            string query =
                "SELECT CIDPRODUCTO, CCODIGOPRODUCTO, CNOMBREPRODUCTO, CPRECIO1, CPRECIO2, CPRECIO3, CPRECIO4, " +
                "CPRECIO5, CPRECIO6,CPRECIO7, CPRECIO8, CPRECIO9, CPRECIO10, CCLAVESAT, CTIPOPRODUCTO " +
                "FROM [adpruebas_de_timbrado].[dbo].[admProductos] " +
                "ORDER BY CCODIGOPRODUCTO DESC;";

            string connString = DatabaseServices.GetConnString();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetInt32(0) == 0 && reader.GetString(1).Trim() == "(Ninguno)")
                        {
                            reader.Read();
                        }

                        List<double> lPrecios = new List<double>();

                        for (int i = 3; i < 13; i++)
                        {
                            double precio = reader.GetDouble(i);
                            if (precio == 0)
                            {
                                break;
                            }

                            lPrecios.Add(precio);
                        }
                        
                        Producto producto;

                        
                        string codigoProducto = reader.GetString(1);
                        string nombreProducto = reader.GetString(2);
                        string claveSAT = reader.GetString(13);
                        int tipoProducto = reader.GetInt32(14);
                        string tipoProductoString = string.Empty;

                        if (tipoProducto == 1)
                        {
                            tipoProductoString = "Productos";
                        }
                        else if (tipoProducto == 2)
                        {
                            tipoProductoString = "Paquete";
                        }
                        else if (tipoProducto == 3)
                        {
                            tipoProductoString = "Servicio";
                        }
                        
                        if (lPrecios.Count > 0)
                        {
                            producto = new Producto(codigoProducto, nombreProducto, claveSAT.ToString(), tipoProductoString, lPrecios);
                        }
                        else
                        {
                            producto = new Producto(codigoProducto, nombreProducto, claveSAT.ToString(), tipoProductoString);
                        }

                        lProductos.Add(producto);
                    }
                }

                return lProductos;
            }
        }

        public List<Cliente> returnClientes()
        {
            List<Cliente> lCliente = new List<Cliente>();
            string codigoCliente, razonSocial, RFC, moneda, tipoCliente = string.Empty;
            int idMoneda, idTipoCliente;
            string query =
                "SELECT CCODIGOCLIENTE, CRAZONSOCIAL, CRFC, admClientes.CIDMONEDA, CTIPOCLIENTE, CNOMBREMONEDA "+
                "FROM [adpruebas_de_timbrado].[dbo].[admClientes] "+
                "INNER JOIN [adpruebas_de_timbrado].[dbo].[admMonedas] " +
                "ON admMonedas.CIDMONEDA = admClientes.CIDMONEDA " +
                "ORDER BY CCODIGOCLIENTE DESC;";

            string connString = DatabaseServices.GetConnString();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        codigoCliente = reader.GetString(0);
                        razonSocial = reader.GetString(1);
                        RFC = reader.GetString(2);
                        idMoneda = reader.GetInt32(3);
                        idTipoCliente = reader.GetInt32(4);
                        tipoCliente = idTipoCliente == 1 ? "Cliente" : "Proveedor";
                        moneda = reader.GetString(5);
                        
                        Cliente cliente = new Cliente(codigoCliente, razonSocial, RFC, idMoneda, moneda, tipoCliente);
                        lCliente.Add(cliente);
                    }
                }

                return lCliente;
            }
        }
        
        public List<InfoDocumento> returnDocumentos(int pageNumber, int rows)
        {
            string query =
                "SELECT CNOMBRECONCEPTO,CCODIGOCONCEPTO, CFOLIO, CSERIEDOCUMENTO, CFECHA, CRAZONSOCIAL, CTOTAL, CPENDIENTE " +
                "FROM [adpruebas_de_timbrado].[dbo].[admDocumentos] " +
                "INNER JOIN  [adpruebas_de_timbrado].[dbo].[admConceptos] " +
                "ON admDocumentos.CIDCONCEPTODOCUMENTO = admConceptos.CIDCONCEPTODOCUMENTO " +
                "ORDER BY CFOLIO DESC " +
                "OFFSET ( @PageNumber - 1) * @RowsOfPage ROWS " +
                "FETCH NEXT @RowsOfPage ROWS ONLY";

            string connString = DatabaseServices.GetConnString();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
                command.Parameters.Add("@RowsOfPage", SqlDbType.Int).Value = rows;

                connection.Open();
                List<InfoDocumento> lDocumento = new List<InfoDocumento>();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        InfoDocumento doc = new InfoDocumento();
                        doc.nombreConcepto = reader.GetString(0).Trim();
                        doc.codConcepto = reader.GetString(1).Trim();
                        doc.folio = Convert.ToInt32(reader.GetDouble(2));
                        doc.serie = reader.GetString(3).Trim();
                        doc.fecha = reader.GetDateTime(4).ToString("MM/dd/yyyy HH:mm:ss");
                        doc.razonSocialCliente = reader.GetString(5).Trim();
                        doc.total = reader.GetDouble(6).ToString().Trim();
                        doc.pendiente = reader.GetDouble(7).ToString().Trim();
                        
                        lDocumento.Add(doc);
                    }
                }
                return lDocumento;
            }
        }
    }
}