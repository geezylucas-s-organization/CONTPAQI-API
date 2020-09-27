using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace CONTPAQ_API.Services
{
    public class DocumentoServices : ContpaqItem
    {
        public bool createDocumento(Documento documento)
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
            lDocto.aFecha = documento.cabecera.fecha;
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
            StringBuilder aValor = new StringBuilder();

            errorCode = SDK.fPosPrimerProducto();

            if (errorCode != 0)
            {
                return lProductos;
            }

            while (SDK.fPosSiguienteProducto() == 0)
            {
                List<double> lPrecios = new List<double>();
                string codigoProducto, nombreProducto;

                // if (errorCode != 0)
                // {
                //     return lProductos;
                // }

                errorCode = SDK.fLeeDatoProducto("cCodigoProducto", aValor, 256);

                if (errorCode != 0)
                {
                    return lProductos;
                }

                codigoProducto = aValor.ToString();

                errorCode = SDK.fLeeDatoProducto("cNombreProducto", aValor, 256);

                if (errorCode != 0)
                {
                    return lProductos;
                }

                nombreProducto = aValor.ToString();

                for (int i = 1; i <= 10; i++)
                {
                    errorCode = SDK.fLeeDatoProducto("cPrecio" + i, aValor, 256);
                    if (errorCode != 0)
                    {
                        return lProductos;
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
            }

            return lProductos;
        }

        public List<Cliente> returnClientes()
        {
            List<Cliente> lCliente = new List<Cliente>();
            StringBuilder aValor = new StringBuilder();

            errorCode = SDK.fPosPrimerCteProv();

            if (errorCode != 0)
            {
                throw new Exception(errorCode + ": " + SDK.rError(errorCode));
            }

            while (SDK.fPosSiguienteCteProv() == 0)
            {
                //errorCode = SDK.fPosSiguienteCteProv();
                string codigoCliente, razonSocial, rfc;
                int moneda;

                // if (errorCode != 0)
                // {
                //     break;
                // }

                errorCode = SDK.fLeeDatoCteProv("cCodigoCliente", aValor, 256);

                if (errorCode != 0)
                {
                    throw new Exception(errorCode + "(CodigoCliente): " + SDK.rError(errorCode));
                }

                codigoCliente = aValor.ToString();

                errorCode = SDK.fLeeDatoCteProv("cRazonSocial", aValor, 256);

                if (errorCode != 0)
                {
                    throw new Exception(errorCode + "(RazonSocial): " + SDK.rError(errorCode));
                }

                razonSocial = aValor.ToString();

                errorCode = SDK.fLeeDatoCteProv("cRFC", aValor, 256);

                if (errorCode != 0)
                {
                    throw new Exception(errorCode + "(RFC): " + SDK.rError(errorCode));
                }

                rfc = aValor.ToString();

                errorCode = SDK.fLeeDatoCteProv("cIDMoneda", aValor, 256);

                if (errorCode != 0)
                {
                    throw new Exception(errorCode + "(IDMoneda): " + SDK.rError(errorCode));
                }

                moneda = Convert.ToInt32(aValor.ToString());

                lCliente.Add(new Cliente(codigoCliente, razonSocial, rfc, moneda));
            }

            return lCliente;
        }

        public List<Concepto> returnConceptos()
        {
            List<Concepto> lConcepto = new List<Concepto>();
            StringBuilder aValor = new StringBuilder();

            errorCode = SDK.fPosPrimerConceptoDocto();

            if (errorCode != 0)
            {
                return lConcepto;
            }

            while (SDK.fPosSiguienteConceptoDocto() == 0)
            {
                string nombreConcepto;
                int noFolio, codigoConcepto;
                errorCode = SDK.fLeeDatoConceptoDocto("cCodigoConcepto", aValor, 256);

                if (errorCode != 0)
                {
                    throw new Exception(errorCode + "(CodigoConcepto): " + SDK.rError(errorCode));
                }

                codigoConcepto = Convert.ToInt32(aValor.ToString());

                if (codigoConcepto != 5)
                {    
                    continue;
                }

                errorCode = SDK.fLeeDatoConceptoDocto("cNombreConcepto", aValor, 256);

                if (errorCode != 0)
                {
                    throw new Exception(errorCode + "(NombreConcepto): " + SDK.rError(errorCode));
                }

                nombreConcepto = aValor.ToString();

                errorCode = SDK.fLeeDatoConceptoDocto("cNoFolio", aValor, 256);

                if (errorCode != 0)
                {
                    throw new Exception(errorCode + "(NoFolio): " + SDK.rError(errorCode));
                }

                noFolio = (int) Convert.ToDouble(aValor.ToString());
                noFolio++;

                lConcepto.Add(new Concepto(codigoConcepto, nombreConcepto, noFolio));
                break;

                //errorCode = SDK.fPosSiguienteConceptoDocto();
            }

            
            
            return lConcepto;
        }

        private InfoDocumento returnDocumentos()
        {
            StringBuilder aValor = new StringBuilder();
            InfoDocumento infoDocumento = new InfoDocumento();
            errorCode = SDK.fLeeDatoDocumento("cIdConceptoDocumento", aValor, 256);

            if (errorCode != 0)
                throw new Exception(errorCode + "(IdConcepto): " + SDK.rError(errorCode));

            errorCode = SDK.fBuscaConceptoDocto(aValor.ToString());

            if (errorCode != 0)
                throw new Exception(errorCode + "(Concepto): " + SDK.rError(errorCode));

            SDK.fLeeDatoConceptoDocto("cCodigoConcepto", aValor, 256);

            if (errorCode != 0)
                throw new Exception(errorCode + "(CodigoConcepto): " + SDK.rError(errorCode));

            infoDocumento.codConcepto = aValor.ToString();

            SDK.fLeeDatoConceptoDocto("cNombreConcepto", aValor, 256);

            if (errorCode != 0)
                throw new Exception(errorCode + "(NombreConcepto): " + SDK.rError(errorCode));

            infoDocumento.nombreConcepto = aValor.ToString();

            errorCode = SDK.fLeeDatoDocumento("cFolio", aValor, 256);

            if (errorCode != 0)
            {
                throw new Exception(errorCode + "(Folio): " + SDK.rError(errorCode));
            }

            infoDocumento.folio = (int) Convert.ToDouble(aValor.ToString());

            errorCode = SDK.fLeeDatoDocumento("cSerieDocumento", aValor, 256);

            if (errorCode != 0)
            {
                throw new Exception(errorCode + "(SerieDocumento): " + SDK.rError(errorCode));
            }

            infoDocumento.serie = aValor.ToString();

            errorCode = SDK.fLeeDatoDocumento("cFecha", aValor, 256);

            if (errorCode != 0)
            {
                throw new Exception(errorCode + "(Fecha): " + SDK.rError(errorCode));
            }

            infoDocumento.fecha = aValor.ToString();

            errorCode = SDK.fLeeDatoDocumento("cRazonSocial", aValor, 256);

            if (errorCode != 0)
            {
                throw new Exception(errorCode + "(RazonSocial): " + SDK.rError(errorCode));
            }

            infoDocumento.razonSocialCliente = aValor.ToString();

            errorCode = SDK.fLeeDatoDocumento("cTotal", aValor, 256);

            if (errorCode != 0)
            {
                throw new Exception(errorCode + "(Total): " + SDK.rError(errorCode));
            }

            infoDocumento.total = aValor.ToString();

            errorCode = SDK.fLeeDatoDocumento("cPendiente", aValor, 256);

            if (errorCode != 0)
            {
                throw new Exception(errorCode + "(Pendiente): " + SDK.rError(errorCode));
            }

            infoDocumento.pendiente = aValor.ToString();

            return infoDocumento;
        }

        public ListOfDocuments returnLastDocumentos(int numberOfDocs)
        {
            List<InfoDocumento> lDocumentos = new List<InfoDocumento>();

            errorCode = SDK.fPosUltimoDocumento();
            if (errorCode != 0)
                throw new Exception(errorCode + "(): " + SDK.rError(errorCode));
            for (int i = 1; i <= numberOfDocs; i++)
            {
                try
                {
                    InfoDocumento infoDocumento = returnDocumentos();
                    lDocumentos.Add(infoDocumento);
                }
                catch (Exception e)
                {
                    throw e;
                }

                if (i != numberOfDocs)
                {
                    errorCode = SDK.fPosAnteriorDocumento();
                    if (errorCode != 0)
                    {
                        if (errorCode == 1)
                            return new ListOfDocuments(lDocumentos, true, true);
                        //return new FunctionReturnedValue(true, lDocumentos, true, true);

                        throw new Exception(errorCode + "(): " + SDK.rError(errorCode));
                    }
                }
            }

            return new ListOfDocuments(lDocumentos, true, false); //last = true; first: false.
        }

        public ListOfDocuments returnFirstDocumentos(int numberOfDocs)
        {
            List<InfoDocumento> lDocumentos = new List<InfoDocumento>();

            errorCode = SDK.fPosPrimerDocumento();
            if (errorCode != 0)
                throw new Exception(errorCode + "(): " + SDK.rError(errorCode));

            for (int i = 1; i <= numberOfDocs; i++)
            {
                try
                {
                    InfoDocumento infoDocumento = returnDocumentos();
                    lDocumentos.Add(infoDocumento);
                }
                catch (Exception e)
                {
                    throw e;
                }

                if (i != numberOfDocs)
                {
                    errorCode = SDK.fPosSiguienteDocumento();
                    if (errorCode != 0)
                    {
                        if (errorCode == 2)
                            return new ListOfDocuments(lDocumentos, true, true);
                        //return new FunctionReturnedValue(true, lDocumentos, true, true);
                        throw new Exception(errorCode + "(): " + SDK.rError(errorCode));
                    }
                }
            }

            return new ListOfDocuments(lDocumentos, false, true); //last: false; first: true.
        }

        public ListOfDocuments returnNextDocumentos(int numberOfDocs)
        {
            List<InfoDocumento> lDocumentos = new List<InfoDocumento>();

            for (int i = 1; i <= numberOfDocs; i++)
            {
                errorCode = SDK.fPosSiguienteDocumento();
                if (errorCode != 0)
                {
                    if (errorCode == 2)
                        return new ListOfDocuments(lDocumentos, true, false); //last: true; first: false.

                    throw new Exception(errorCode + "(): " + SDK.rError(errorCode));
                }
 
                try
                {
                    InfoDocumento infoDocumento = returnDocumentos();
                    lDocumentos.Add(infoDocumento);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            if (SDK.fPosSiguienteDocumento() != 0)
            {
                if (errorCode == 2)
                {
                    return new ListOfDocuments(lDocumentos, true, true);
                    //last: true; first: false;
                }
            }

            SDK.fPosAnteriorDocumento();
            return new ListOfDocuments(lDocumentos, false, false); //last: false; first: false;
        }

        public ListOfDocuments returnPrevDocumentos(int numberOfDocs)
        {
            List<InfoDocumento> lDocumentos = new List<InfoDocumento>();

            for (int i = 1; i <= numberOfDocs; i++)
            {
                errorCode = SDK.fPosAnteriorDocumento();
                if (errorCode != 0)
                {
                    if (errorCode == 1)
                        return new ListOfDocuments(lDocumentos, false, true);
                    ; //last: false; first: true.

                    throw new Exception(errorCode + "(): " + SDK.rError(errorCode));
                }

                try
                {
                    InfoDocumento infoDocumento = returnDocumentos();
                    lDocumentos.Add(infoDocumento);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            
            if (SDK.fPosAnteriorDocumento() != 0)
            {
                if (errorCode == 2)
                {
                    return new ListOfDocuments(lDocumentos, true, false);
                    //last: true; first: false;
                }
            }

            errorCode = SDK.fPosSiguienteDocumento();

            return new ListOfDocuments(lDocumentos, false, false); //last: false; first: false.
        }

        public bool moveForwardsDocumentos(int numberOfDocs)
        {
            int lError;
            for (int i = 1; i < numberOfDocs; i++)
            {
                lError = SDK.fPosSiguienteDocumento();
                if (lError != 0)
                    return false;
            }

            return true;
        }

        public bool moveBackwardsDocumentos(int numberOfDocs)
        {
            int lError;
            for (int i = 1; i < numberOfDocs; i++)
            {
                lError = SDK.fPosAnteriorDocumento();
                if (lError != 0)
                    return false;
            }

            return true;
        }
    }
}