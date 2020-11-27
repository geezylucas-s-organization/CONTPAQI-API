using System;
using System.Collections.Generic;
using CONTPAQ_API.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace CONTPAQ_API.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("api/[controller]")]
    public class ClienteController : Controller
    {
        [HttpPost] // post api/Cliente
        public IActionResult create([FromBody] ClienteJSON clienteJson)
        {
            ClienteServices clienteServices = new ClienteServices();
            if (clienteServices.create(clienteJson))
            {
                return StatusCode(201);
            }

            if (clienteServices.errorCode == 120502)
            {
                return StatusCode(400, clienteServices.errorMessage);
            }

            return StatusCode(500, clienteServices.errorMessage);
        }

        [HttpGet("GetClientes")] // post api/Cliente/GetClientes
        public ActionResult getClientes([FromQuery(Name = "PageNumber")] int PageNumber,
            [FromQuery(Name = "Rows")] int Rows)
        {
            ClienteServices clienteServices = new ClienteServices();
            List<Cliente> lCliente = clienteServices.returnClientes(PageNumber, Rows);
            double total = 0;

            string query = "SELECT COUNT(*) AS 'TOTAL' FROM [adpruebas_de_timbrado].[dbo].[admClientes];";

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

            double totalPages = total / Rows;
            totalPages = Math.Ceiling(totalPages);

            ListOfClientes listOfClientes = new ListOfClientes(lCliente, PageNumber, Convert.ToInt32(totalPages));
            string jsonString;
            jsonString = JsonSerializer.Serialize(listOfClientes);
            return Ok(jsonString);
        }
    }
}