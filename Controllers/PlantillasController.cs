using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CONTPAQ_API.Models;
using CONTPAQ_API.Models.DB;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CONTPAQ_API.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("api/[controller]")]
    public class PlantillasController : Controller
    {
        [HttpPatch("Documento")]
        public ActionResult editDocumento(int DocumentoId, [FromBody] JsonPatchDocument<Documentos> documentoChanges)
        {
            PlantillasContext db = new PlantillasContext();
            if (documentoChanges == null)
            {
                return BadRequest();
            }

            Documentos doc = db.Documentos.FirstOrDefault(x => x.Documentoid == DocumentoId);

            if (doc == null)
            {
                return NotFound();
            }

            documentoChanges.ApplyTo(doc, ModelState);

            var isValid = TryValidateModel(doc);

            if (!isValid)
            {
                return BadRequest();
            }

            db.SaveChanges();
            return NoContent();
        }

        [HttpPatch("Movimientos")]
        public ActionResult editMovimientos(int DocumentoId, int MovimientoId,
            [FromBody] JsonPatchDocument<Movimientos> movimientosChanges)
        {
            PlantillasContext db = new PlantillasContext();
            if (movimientosChanges == null)
            {
                return BadRequest();
            }

            Movimientos movimiento =
                db.Movimientos.FirstOrDefault(x => x.Documentoid == DocumentoId && x.NumeroMovimiento == MovimientoId);

            if (movimiento == null)
            {
                return NotFound();
            }

            movimientosChanges.ApplyTo(movimiento, ModelState);

            var isValid = TryValidateModel(movimiento);

            if (!isValid)
            {
                return BadRequest();
            }

            db.SaveChanges();
            return NoContent();
        }

        [HttpPatch("Cabecera")]
        public ActionResult editCabecera(int DocumentoId, [FromBody] JsonPatchDocument<Cabeceras> cabeceraChanges)
        {
            PlantillasContext db = new PlantillasContext();
            if (cabeceraChanges == null)
            {
                return BadRequest();
            }

            Cabeceras cabecera = db.Cabeceras.FirstOrDefault(x => x.Documentoid == DocumentoId);

            if (cabecera == null)
            {
                return NotFound();
            }

            cabeceraChanges.ApplyTo(cabecera, ModelState);

            var isValid = TryValidateModel(cabecera);

            if (!isValid)
            {
                return BadRequest();
            }

            db.SaveChanges();
            return NoContent();
        }

        [HttpGet]
        public ActionResult getPlantillas(int? DocumentoId, int Page, int Size)
        {
            PlantillasContext db = new PlantillasContext();
            string jsonresponse;

            if (DocumentoId != null)
            {
                Documentos documento = db.Documentos.FirstOrDefault(x => x.Documentoid == DocumentoId);

                if (documento == null)
                {
                    return BadRequest("El id de la plantilla no existe.");
                }

                documento.Cabeceras = db.Cabeceras.FirstOrDefault(x => x.Documentoid == DocumentoId);

                documento.Movimientos = db.Movimientos.Where(x => x.Documentoid == DocumentoId).ToList();

                Plantilla plantillaCompleta = new Plantilla(documento);

                jsonresponse = JsonSerializer.Serialize(plantillaCompleta);

                return Ok(jsonresponse);
            }

            List<DocumentoPlantilla> documentosPlantilla = new List<DocumentoPlantilla>();

            try
            {
                var plantillas = (from doc in db.Documentos
                    join cabecera in db.Cabeceras on doc.Documentoid equals cabecera.Documentoid
                    select new {doc, codigoClienteProveedor = cabecera.CodigoCteProv}).Skip((Page - 1) * Size).Take(Size);
                
                foreach (var item in plantillas)
                {
                    Documentos documento = item.doc;
                    DocumentoPlantilla doc = new DocumentoPlantilla(documento, item.codigoClienteProveedor);
                    documentosPlantilla.Add(doc);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            
            jsonresponse = JsonSerializer.Serialize(documentosPlantilla);

            return Ok(jsonresponse);
        }
    }
}