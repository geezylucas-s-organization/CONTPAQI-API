using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CONTPAQ_API.Models;
using CONTPAQ_API.Models.DB;
using Hangfire;
using CONTPAQ_API.Services;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CONTPAQ_API.Services
{
    public class PlantillasServices
    {
        public static void initializeHangfire()
        {
        }

        public static void func()
        {
            RecurringJob.AddOrUpdate(() => checkForPlantillas(), Cron.Daily(), TimeZoneInfo.Local);

            using (var server = new BackgroundJobServer())
            {
                //Console.ReadLine();
            }
        }

        public static bool addPlantilla(Documento documento)
        {
            using (var db = new PlantillasContext())
            {
                int noPlantillas = db.Documentos.Count();
                int nextId;

                if (noPlantillas != 0)
                {
                    nextId = db.Documentos.Max(x => x.Documentoid) + 1;
                }
                else
                {
                    nextId = 1;
                }
                
                var factura = new Documentos
                {
                    Documentoid = nextId,
                    Estatus = false
                };
                try
                {
                    db.Documentos.Add(factura);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                DateTime fechaCabecera = DateTime.ParseExact(documento.cabecera.fecha,"MM/dd/yyyy",CultureInfo.InvariantCulture);
                
                var cabecera = new Cabeceras
                {
                    Documentoid = factura.Documentoid,
                    NumMoneda = documento.cabecera.numMoneda,
                    Serie = documento.cabecera.serie.ToString(),
                    TipoCambio = documento.cabecera.tipoCambio,
                    CodConcepto = documento.cabecera.codConcepto,
                    CodigoCteProv = documento.cabecera.codigoCteProv
                };

                db.Cabeceras.Add(cabecera);

                for (int i = 1; i < documento.movimientos.Count; i++)
                {
                    var movimientodb = new Movimientos
                    {
                        Documentoid = factura.Documentoid,
                        NumeroMovimiento = i,
                        CodAlmacen = documento.movimientos[i].codAlmacen,
                        CodProducto = documento.movimientos[i].codProducto,
                        Precio = documento.movimientos[i].precio,
                        Unidades = documento.movimientos[i].unidades
                    };
                    db.Movimientos.Add(movimientodb);
                }
                
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                
            }

            return true;
        }

        public static void checkForPlantillas()
        {
            using (var db = new PlantillasContext())
            {
                var documentosPlantillas = db.Documentos.ToList();
                foreach (var documento in documentosPlantillas)
                {
                    if (!documento.Estatus) continue;
                    if (documento.ProximaFactura == DateTime.Today)
                    {
                        int hora = documento.ProximaFactura.Value.Hour;
                        DocumentoServices documentoServices = new DocumentoServices();
                        var cabecera = db.Cabeceras.FirstOrDefault(x => x.Documentoid == documento.Documentoid);
                        var movimiento = db.Movimientos.Where(x => x.Documentoid == documento.Documentoid).ToList();

                        Documento factura = new Documento
                        {
                            cabecera = new Cabecera
                            {
                                numMoneda = cabecera.NumMoneda,
                                serie = new StringBuilder(cabecera.Serie),
                                tipoCambio = cabecera.TipoCambio,
                                codConcepto = cabecera.CodConcepto,
                                codigoCteProv = cabecera.CodigoCteProv
                            },
                            movimientos = new List<Movimiento>()
                        };

                        movimiento.ForEach(x => factura.movimientos.Add(new Movimiento
                        {
                            codAlmacen = x.CodAlmacen,
                            codProducto = x.CodProducto,
                            precio = x.Precio,
                            unidades = x.Unidades
                        }));

                        factura.docEnPlantiila.isPlantilla = true;
                        factura.docEnPlantiila.idPlantilla = documento.Documentoid;

                        BackgroundJob.Schedule(() => documentoServices.createDocumento(factura),
                            TimeSpan.FromHours(hora));
                    }
                }
            }
        }

        public static bool updatePlantilla(Documento documento)
        {
            PlantillasContext db = new PlantillasContext();
            Documentos doc =
                db.Documentos.FirstOrDefault(x => x.Documentoid == documento.docEnPlantiila.idPlantilla);
            try
            {
                doc.ProximaFactura.Value.AddDays(doc.PeriodoDias.Value);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}