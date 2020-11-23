using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
//using CONTPAQ_API.Models;
using CONTPAQ_API.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;

namespace CONTPAQ_API.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("api/[controller]")]
    public class DocumentoController : Controller
    {
        private string prevAction = string.Empty;

        [HttpGet("FillView")] // GET api/Documento/FillView
        public ActionResult FillCreateDocumentView()
        {
            DocumentoServices documentoServices = new DocumentoServices();
            CreateDocumentoView respuesta = new CreateDocumentoView();
            try
            {
                respuesta.productosYServicios = documentoServices.returnProductos();
                respuesta.clientesYProveedores = documentoServices.returnClientes();
                respuesta.conceptos = documentoServices.returnConceptos();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

            string jsonString;
            jsonString = JsonSerializer.Serialize(respuesta);
            //PlantillasServices.func();

            return Ok(jsonString);
        }

        [HttpGet("GetDocumentos")] // GET api/Documento/GetDocumentos
        public ActionResult GetDocumentos([FromQuery(Name = "PageNumber")] int PageNumber,
            [FromQuery(Name = "Rows")] int Rows)
        {
            DocumentoServices documentoServices = new DocumentoServices();
            List<InfoDocumento> listOfDocuments = documentoServices.returnDocumentos(PageNumber, Rows);

            string jsonString;
            jsonString = JsonSerializer.Serialize(listOfDocuments);

            return Ok(jsonString);
        }

        [HttpPost] // POST api/Documento/
        public ActionResult Post([FromBody] Documento documento)
        {
            DocumentoServices documentoServices = new DocumentoServices();
            if (!documentoServices.createDocumento(documento))
                return StatusCode(500, documentoServices.errorMessage);

            return StatusCode(201);
        }
    }
}