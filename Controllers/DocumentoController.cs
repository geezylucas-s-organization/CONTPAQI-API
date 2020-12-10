using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
//using CONTPAQ_API.Models;
using CONTPAQ_API.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Data.SqlClient;

namespace CONTPAQ_API.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("api/[controller]")]
    public class DocumentoController : Controller
    {
        private string prevAction = string.Empty;
        
        [HttpGet("GetDocumentos")] // GET api/Documento/GetDocumentos
        public ActionResult GetDocumentos([FromQuery(Name = "PageNumber")] int PageNumber,
            [FromQuery(Name = "Rows")] int Rows)
        {
            DocumentoServices documentoServices = new DocumentoServices();
            List<InfoDocumento> documentos = documentoServices.returnDocumentos(PageNumber, Rows);
            int total = 0;

            string query = "SELECT COUNT(*) AS 'TOTAL' FROM [adpruebas_de_timbrado].[dbo].[admDocumentos];";

            string connString = DatabaseServices.GetConnString();

            using (SqlConnection sqlConnection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        total = reader.GetInt32(0);
                    }
                }
            }

            ListOfDocuments listOfDocuments = new ListOfDocuments(documentos, PageNumber, total);
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