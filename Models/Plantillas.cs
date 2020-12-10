using System;
using System.Collections.Generic;
using System.Text;
using CONTPAQ_API.Models.DB;
using Microsoft.AspNetCore.Components.Web;

namespace CONTPAQ_API.Models
{
    public class Plantilla
    {
        public DocumentoPlantilla documentoPlantilla { get; set; }
        public CabeceraPlantilla cabeceraPlantilla { get; set; }
        public List<MovimientoPlantilla> movimientosPlantilla { get; set; }

        public Plantilla(Documentos documento)
        {
            movimientosPlantilla = new List<MovimientoPlantilla>();
            documentoPlantilla = new DocumentoPlantilla(documento);
            cabeceraPlantilla = new CabeceraPlantilla(documento.Cabeceras);
            foreach (var movimiento in documento.Movimientos)
            {
                movimientosPlantilla.Add(new MovimientoPlantilla(movimiento));
            }

            // documentoPlantilla.ClienteProveedor = cabeceraPlantilla.ClienteProveedor;
        }
    }

    public class DocumentoPlantilla
    {
        public int Documentoid { get; set; }
        public string Descripcion { get; set; }
        public DateTime? UltimaVezFacturada { get; set; }
        public DateTime? ProximaFactura { get; set; }
        public bool Estatus { get; set; }
        public string ClienteProveedor { get; set; }

        public DocumentoPlantilla(Documentos documentos)
        {
            Documentoid = documentos.Documentoid;
            Descripcion = documentos.Descripcion;
            if (documentos.UltimaVezFacturada != null)
            {
                UltimaVezFacturada = documentos.UltimaVezFacturada;
            }

            if (documentos.ProximaFactura != null)
            {
                ProximaFactura = documentos.ProximaFactura;
            }

            Estatus = documentos.Estatus;
        }

        public DocumentoPlantilla(Documentos documento, string codigoCteProv) : this(documento)
        {
            if (SDK.fBuscaCteProv(codigoCteProv) == 0) //Si encontró el Cliente/Proveedor
            {
                StringBuilder aValor = new StringBuilder();
                if (SDK.fLeeDatoCteProv("cRazonSocial", aValor, 256) == 0)
                {
                    ClienteProveedor = aValor.ToString();
                }
            }
        }
    }

    public class CabeceraPlantilla
    {
        public int Documentoid { get; set; }
        public int NumMoneda { get; set; }
        public string Serie { get; set; }
        public int TipoCambio { get; set; }
        public string CodConcepto { get; set; }
        public string CodigoCteProv { get; set; }

        public CabeceraPlantilla(Cabeceras cabecera)
        {
            Documentoid = cabecera.Documentoid;
            NumMoneda = cabecera.NumMoneda;
            Serie = cabecera.Serie;
            TipoCambio = cabecera.TipoCambio;
            CodConcepto = cabecera.CodConcepto;
            CodigoCteProv = cabecera.CodigoCteProv;

            // if (SDK.fBuscaCteProv(CodigoCteProv) == 0) //Si encontró el Cliente/Proveedor
            // {
            //     StringBuilder aValor = new StringBuilder();
            //     if (SDK.fLeeDatoCteProv("cRazonSocial", aValor, 256) == 0)
            //     {
            //         ClienteProveedor = aValor.ToString();
            //     }
            // }
        }
    }

    public class MovimientoPlantilla
    {
        public int Documentoid { get; set; }
        public int NumeroMovimiento { get; set; }
        public string CodAlmacen { get; set; }
        public string CodProducto { get; set; }
        public double Precio { get; set; }
        public int Unidades { get; set; }

        public MovimientoPlantilla(Movimientos movimientos)
        {
            Documentoid = movimientos.Documentoid;
            NumeroMovimiento = movimientos.NumeroMovimiento;
            CodAlmacen = movimientos.CodAlmacen;
            CodProducto = movimientos.CodProducto;
            Precio = movimientos.Precio;
            Unidades = movimientos.Unidades;
        }
    }
    public class ListOfPlantillas
    {
        public int page { get; set; }
        public int total { get; set; }
        public List<DocumentoPlantilla> data { get; set; }


        public ListOfPlantillas(List<DocumentoPlantilla> infoPlantilla, int page, int total)
        {
            this.data = infoPlantilla;
            this.page = page;
            this.total = total;
        }
    }
}