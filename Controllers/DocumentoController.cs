using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using CONTPAQ_API.Services;

namespace CONTPAQ_API.Controllers
{
    [ApiController]
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
        public ActionResult GetDocumentos([FromBody] InfoRequired infoRequired)
        {
            // FunctionReturnedValue functionReturnedValue = SDKServices.Conectar();
            //
            // if (!functionReturnedValue.isValid)
            // {
            //     return StatusCode(500, functionReturnedValue.message);
            // }

            FunctionReturnedValue functionReturnedValue;

            switch (infoRequired.action)
            {
                case "last":
                    functionReturnedValue = DocumentoServices.returnLastDocumentos(infoRequired.numberOfDocs);
                    break;
                case "first":
                    functionReturnedValue = DocumentoServices.returnFirstDocumentos(infoRequired.numberOfDocs);
                    break;
                case "next":
                    if (prevAction == "prev")
                        DocumentoServices.moveForwardsDocumentos(infoRequired.numberOfDocs);

                    functionReturnedValue = DocumentoServices.returnNextDocumentos(infoRequired.numberOfDocs);
                    break;
                case "prev":
                    if (prevAction == "next")
                        DocumentoServices.moveBackwardsDocumentos(infoRequired.numberOfDocs);

                    functionReturnedValue = DocumentoServices.returnPrevDocumentos(infoRequired.numberOfDocs);
                    break;
                default:
                    return new StatusCodeResult(404);
            }

            prevAction = infoRequired.action;

            ResponseListOfDocuments responseListOfDocuments = new ResponseListOfDocuments(
                functionReturnedValue.infoDocumentos, functionReturnedValue.isLast, functionReturnedValue.isFirst);

            string jsonString;
            jsonString = JsonSerializer.Serialize(responseListOfDocuments);
            return Ok(jsonString);
        }

        [HttpPost("CreateDocumento")] // POST api/Documento/CreateDocumento
        public ActionResult Post([FromBody] Documento documento)
        {
            //documento = JsonSerializer.Deserialize<Documento>(doc);
            //FunctionReturnedValue functionReturnedValue = SDKServices.Conectar();
            // if (functionReturnedValue.isValid)
            // {
            //     DocumentoServices.CreaDocto(documento);
            // }
            // else
            // {
            //     return StatusCode(500, functionReturnedValue.message);
            // }


            FunctionReturnedValue functionReturnedValue = DocumentoServices.CreaDocto(documento);

            if (!functionReturnedValue.isValid)
                return StatusCode(500, functionReturnedValue.message);

            //CreaDoctoPago();
            //SDKServices.Termina();
            return StatusCode(201);
        }
    }
}