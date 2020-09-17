using System;
using System.Collections.Generic;
using System.Text;

namespace CONTPAQ_API.Services
{
    public class DocumentoServices
    {
        public static FunctionReturnedValue createDocumento(Documento documento)
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

            //Paso 5: Se exporta a XML o PDF

            lError = SDK.fEntregEnDiscoXML(documento.cabecera.codConcepto,
                documento.cabecera.serie.ToString(), documento.cabecera.folio, 0, "");
            if (lError != 0)
            {
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }


            lError = SDK.fEntregEnDiscoXML(documento.cabecera.codConcepto,
                documento.cabecera.serie.ToString(), documento.cabecera.folio, 1, "");
            if (lError != 0)
            {
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }


            return new FunctionReturnedValue(true);
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

        public static FunctionReturnedValue returnProductos()
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
                List<double> lPrecios = new List<double>();
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

                for (int i = 1; i <= 10; i++)
                {
                    lError = SDK.fLeeDatoProducto("cPrecio" + i, aValor, 256);
                    if (lError != 0)
                    {
                        return new FunctionReturnedValue(false, SDK.rError(lError));
                    }

                    if (Convert.ToDouble(aValor.ToString()) == 0)
                    {
                        break;
                    }

                    lPrecios.Add(Convert.ToDouble(aValor.ToString()));
                }

                if (lPrecios.Count == 0)
                {
                    lProductos.Add(new Producto(codigoProducto, nombreProducto));
                }
                else
                {
                    lProductos.Add(new Producto(codigoProducto, nombreProducto, lPrecios));
                }
            } while (lError == 0);

            return new FunctionReturnedValue(true, lProductos);
        }

        public static FunctionReturnedValue returnClientes()
        {
            List<Cliente> lCliente = new List<Cliente>();
            StringBuilder aValor = new StringBuilder();
            string codigoCliente, razonSocial, rfc;
            int lError, moneda;

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
                moneda = 0;

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

                lError = SDK.fLeeDatoCteProv("cRFC", aValor, 256);

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

                moneda = Convert.ToInt32(aValor.ToString());

                lCliente.Add(new Cliente(codigoCliente, razonSocial, rfc, moneda));
            } while (lError == 0);

            return new FunctionReturnedValue(true, lCliente);
        }

        public static FunctionReturnedValue returnConceptos()
        {
            List<Concepto> lConcepto = new List<Concepto>();
            StringBuilder aValor = new StringBuilder();
            string nombreConcepto;
            int lError, noFolio, codigoConcepto;

            lError = SDK.fPosPrimerConceptoDocto();

            if (lError != 0)
            {
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }

            do
            {
                lError = SDK.fLeeDatoConceptoDocto("cCodigoConcepto", aValor, 256);

                if (lError != 0)
                {
                    return new FunctionReturnedValue(false, SDK.rError(lError));
                }

                codigoConcepto = Convert.ToInt32(aValor.ToString());

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

                noFolio = (int) Convert.ToDouble(aValor.ToString());
                noFolio++;

                lConcepto.Add(new Concepto(codigoConcepto, nombreConcepto, noFolio));

                lError = SDK.fPosSiguienteConceptoDocto();
                codigoConcepto = 0;
                nombreConcepto = string.Empty;
                noFolio = 0;
                if (lError != 0)
                {
                    break;
                }
            } while (lError == 0);

            return new FunctionReturnedValue(true, lConcepto);
        }

        private static FunctionReturnedValue returnDocumentos()
        {
            StringBuilder aValor = new StringBuilder();
            int lError;
            InfoDocumento infoDocumento = new InfoDocumento();
            lError = SDK.fLeeDatoDocumento("cIdConceptoDocumento", aValor, 256);

            if (lError != 0)
                return new FunctionReturnedValue(false, SDK.rError(lError));

            lError = SDK.fBuscaConceptoDocto(aValor.ToString());

            if (lError != 0)
                return new FunctionReturnedValue(false, SDK.rError(lError));

            SDK.fLeeDatoConceptoDocto("cCodigoConcepto", aValor, 256);

            if (lError != 0)
                return new FunctionReturnedValue(false, SDK.rError(lError));

            infoDocumento.codConcepto = aValor.ToString();

            SDK.fLeeDatoConceptoDocto("cNombreConcepto", aValor, 256);

            if (lError != 0)
                return new FunctionReturnedValue(false, SDK.rError(lError));

            infoDocumento.nombreConcepto = aValor.ToString();

            lError = SDK.fLeeDatoDocumento("cFolio", aValor, 256);

            if (lError != 0)
            {
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }

            infoDocumento.folio = (int) Convert.ToDouble(aValor.ToString());

            lError = SDK.fLeeDatoDocumento("cSerieDocumento", aValor, 256);

            if (lError != 0)
            {
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }

            infoDocumento.serie = aValor.ToString();
            
            lError = SDK.fLeeDatoDocumento("cFecha", aValor, 256);

            if (lError != 0)
            {
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }

            infoDocumento.fecha = aValor.ToString();

            lError = SDK.fLeeDatoDocumento("cRazonSocial", aValor, 256);

            if (lError != 0)
            {
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }

            infoDocumento.razonSocialCliente = aValor.ToString();

            lError = SDK.fLeeDatoDocumento("cTotal", aValor, 256);

            if (lError != 0)
            {
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }

            infoDocumento.total = aValor.ToString();

            lError = SDK.fLeeDatoDocumento("cPendiente", aValor, 256);

            if (lError != 0)
            {
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }

            infoDocumento.pendiente = aValor.ToString();

            return new FunctionReturnedValue(true, infoDocumento);
        }

        public static FunctionReturnedValue returnLastDocumentos(int numberOfDocs)
        {
            int lError;
            List<InfoDocumento> lDocumentos = new List<InfoDocumento>();

            lError = SDK.fPosUltimoDocumento();
            if (lError != 0)
                return new FunctionReturnedValue(false, SDK.rError(lError));

            for (int i = 1; i <= numberOfDocs; i++)
            {
                FunctionReturnedValue functionReturnedValue = returnDocumentos();
                if (!functionReturnedValue.isValid)
                    return functionReturnedValue;

                lDocumentos.Add(functionReturnedValue.infoDocumento);

                if (i != numberOfDocs)
                {
                    lError = SDK.fPosAnteriorDocumento();
                    if (lError != 0)
                    {
                        if (lError == 1)
                            return new FunctionReturnedValue(true, lDocumentos, true, true);
                        
                        return new FunctionReturnedValue(false, SDK.rError(lError));
                    }

                }
                
            }
            return new FunctionReturnedValue(true, lDocumentos, true, false);
        }

        public static FunctionReturnedValue returnFirstDocumentos(int numberOfDocs)
        {
            int lError;
            List<InfoDocumento> lDocumentos = new List<InfoDocumento>();

            lError = SDK.fPosPrimerDocumento();
            if (lError != 0)
                return new FunctionReturnedValue(false, SDK.rError(lError));

            for (int i = 1; i <= numberOfDocs; i++)
            {
                FunctionReturnedValue functionReturnedValue = returnDocumentos();
                if (!functionReturnedValue.isValid)
                    return functionReturnedValue;

                lDocumentos.Add(functionReturnedValue.infoDocumento);

                if (i != numberOfDocs)
                {
                    lError = SDK.fPosSiguienteDocumento();
                    if (lError != 0)
                    {
                        if (lError == 2)
                            return new FunctionReturnedValue(true, lDocumentos, true, true);
                        return new FunctionReturnedValue(false, SDK.rError(lError));   

                    }
                }
            }
            return new FunctionReturnedValue(true, lDocumentos, false, true);
        }

        public static FunctionReturnedValue returnNextDocumentos(int numberOfDocs)
        {
            int lError;
            List<InfoDocumento> lDocumentos = new List<InfoDocumento>();

            for (int i = 1; i <= numberOfDocs; i++)
            {
                lError = SDK.fPosSiguienteDocumento();
                if (lError != 0)
                {
                    if (lError == 2)
                        return new FunctionReturnedValue(true, lDocumentos, true, false);

                    return new FunctionReturnedValue(false, SDK.rError(lError));
                }
                
                FunctionReturnedValue functionReturnedValue = returnDocumentos();
                if (!functionReturnedValue.isValid)
                    return functionReturnedValue;

                lDocumentos.Add(functionReturnedValue.infoDocumento);
            }
            lError = SDK.fPosSiguienteDocumento();

            if (lError != 0)
            {
                if (lError == 2)
                    return new FunctionReturnedValue(true, lDocumentos, true, false);
                
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }
            
            lError = SDK.fPosAnteriorDocumento();

            if (lError != 0)
                return new FunctionReturnedValue(false, SDK.rError(lError));

            return new FunctionReturnedValue(true, lDocumentos, false, false);
            
        }

        public static FunctionReturnedValue returnPrevDocumentos(int numberOfDocs)
        {
            int lError;
            List<InfoDocumento> lDocumentos = new List<InfoDocumento>();

            for (int i = 1; i <= numberOfDocs; i++)
            {
                lError = SDK.fPosAnteriorDocumento();
                if (lError != 0)
                {
                    if (lError == 1)
                        return new FunctionReturnedValue(true, lDocumentos, false, true);
                    
                    return new FunctionReturnedValue(false, SDK.rError(lError));
                }
                
                FunctionReturnedValue functionReturnedValue = returnDocumentos();
                if (!functionReturnedValue.isValid)
                    return functionReturnedValue;

                lDocumentos.Add(functionReturnedValue.infoDocumento);
            }
            
            //Validación.

            lError = SDK.fPosAnteriorDocumento();

            if (lError != 0)
            {
                if (lError == 1)
                    return new FunctionReturnedValue(true, lDocumentos, false, true);
                
                return new FunctionReturnedValue(false, SDK.rError(lError));
            }
            
            lError = SDK.fPosSiguienteDocumento();

            if (lError != 0)
                return new FunctionReturnedValue(false, SDK.rError(lError));

            return new FunctionReturnedValue(true, lDocumentos, false, false);
        }

        public static FunctionReturnedValue moveForwardsDocumentos(int numberOfDocs)
        {
            int lError;
            for (int i = 1; i <  numberOfDocs; i++)
            {
                lError = SDK.fPosSiguienteDocumento();
                if (lError != 0)
                    return new FunctionReturnedValue(false, SDK.rError(lError));
            }
            return new FunctionReturnedValue(true);
        }
        
        public static FunctionReturnedValue moveBackwardsDocumentos(int numberOfDocs)
        {
            int lError;
            for (int i = 1; i < numberOfDocs; i++)
            {
                lError = SDK.fPosAnteriorDocumento();
                if (lError != 0)
                    return new FunctionReturnedValue(false, SDK.rError(lError));
            }
            return new FunctionReturnedValue(true);
        }
    }
}