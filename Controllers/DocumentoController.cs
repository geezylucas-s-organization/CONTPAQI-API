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
        private static string prevAction = string.Empty;

        [HttpGet("FillView")] // GET api/Documento/FillView
        public ActionResult FillCreateDocumentView()
        {
            // FunctionReturnedValue functionReturnedValue = SDKServices.Conectar();
            //
            // if (!functionReturnedValue.isValid)
            //     return StatusCode(500, functionReturnedValue.message);

            FunctionReturnedValue functionReturnedValue = DocumentoServices.returnProductos();

            if (!functionReturnedValue.isValid)
                return StatusCode(500, functionReturnedValue.message);

            CreateDocumentoView respuesta = new CreateDocumentoView();
            respuesta.productosYServicios = functionReturnedValue.productos;

            functionReturnedValue = DocumentoServices.returnClientes();

            if (!functionReturnedValue.isValid)
                return StatusCode(500, functionReturnedValue.message);

            respuesta.clientesYProveedores = functionReturnedValue.clientes;
            functionReturnedValue = DocumentoServices.returnConceptos();

            if (!functionReturnedValue.isValid)
                return StatusCode(500, functionReturnedValue.message);

            respuesta.conceptos = functionReturnedValue.conceptos;
            string jsonString;
            jsonString = JsonSerializer.Serialize(respuesta);

            //SDKServices.Termina();
            return Ok(jsonString);
        }

        [HttpGet("GetDocumentos")] // GET api/Documento/GetDocumentos
        public ActionResult GetDocumentos([FromQuery (Name = "action")] string action, [FromQuery (Name = "numberOfDocs")] int numberOfDocs)
        {
            // FunctionReturnedValue functionReturnedValue = SDKServices.Conectar();
            //
            // if (!functionReturnedValue.isValid)
            // {
            //     return StatusCode(500, functionReturnedValue.message);
            // }

            FunctionReturnedValue functionReturnedValue;

            switch (action)
            {
                case "last":
                    functionReturnedValue = DocumentoServices.returnLastDocumentos(numberOfDocs);
                    break;
                case "first":
                    functionReturnedValue = DocumentoServices.returnFirstDocumentos(numberOfDocs);
                    break;
                case "next":
                    if (prevAction == "prev")
                        DocumentoServices.moveForwardsDocumentos(numberOfDocs);

                    functionReturnedValue = DocumentoServices.returnNextDocumentos(numberOfDocs);
                    break;
                case "prev":
                    if (prevAction == "next")
                        DocumentoServices.moveBackwardsDocumentos(numberOfDocs);

                    functionReturnedValue = DocumentoServices.returnPrevDocumentos(numberOfDocs);
                    break;
                default:
                    return new StatusCodeResult(404);
            }

            prevAction = action;

            ResponseListOfDocuments responseListOfDocuments = new ResponseListOfDocuments(
                functionReturnedValue.infoDocumentos, functionReturnedValue.isLast, functionReturnedValue.isFirst);

            string jsonString;
            jsonString = JsonSerializer.Serialize(responseListOfDocuments);
            return Ok(jsonString);
        }

        [HttpPost] // POST api/Documento/CreateDocumento
        public ActionResult Post([FromBody] Documento documento)
        {
            FunctionReturnedValue functionReturnedValue = DocumentoServices.createDocumento(documento);

            if (!functionReturnedValue.isValid)
                return StatusCode(500, functionReturnedValue.message);

            //CreaDoctoPago();
            //SDKServices.Termina();
            return StatusCode(201);
        }
    }
}