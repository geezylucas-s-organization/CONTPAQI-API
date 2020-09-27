using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using CONTPAQ_API.Services;
using Microsoft.AspNetCore.Cors;

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
            // catch (CustomException e)
            // {
            //     
            // }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

            string jsonString;
            jsonString = JsonSerializer.Serialize(respuesta);
            PlantillasServices.func();
            
            return Ok(jsonString);
        }

        [HttpGet("GetDocumentos")] // GET api/Documento/GetDocumentos
        public ActionResult GetDocumentos([FromQuery(Name = "action")] string action,
            [FromQuery(Name = "numberOfDocs")] int numberOfDocs)
        {
            // FunctionReturnedValue functionReturnedValue = SDKServices.Conectar();
            //
            // if (!functionReturnedValue.isValid)
            // {
            //     return StatusCode(500, functionReturnedValue.message);
            // }

            DocumentoServices documentoServices = new DocumentoServices();
            ListOfDocuments listOfDocuments;

            switch (action)
            {
                case "last":
                    listOfDocuments = documentoServices.returnLastDocumentos(numberOfDocs);
                    break;
                case "first":
                    listOfDocuments = documentoServices.returnFirstDocumentos(numberOfDocs);
                    break;
                case "next":
                    if (prevAction == "prev")
                        documentoServices.moveForwardsDocumentos(numberOfDocs);

                    listOfDocuments = documentoServices.returnNextDocumentos(numberOfDocs);
                    break;
                case "prev":
                    if (prevAction == "next")
                        documentoServices.moveBackwardsDocumentos(numberOfDocs);

                    listOfDocuments = documentoServices.returnPrevDocumentos(numberOfDocs);
                    break;
                default:
                    return new StatusCodeResult(404);
            }

            prevAction = action;

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