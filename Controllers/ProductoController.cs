using System;
using System.Collections.Generic;
using System.Text.Json;
using CONTPAQ_API.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CONTPAQ_API.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("api/[controller]")]
    public class ProductoController : Controller
    {
        [HttpPost] // POST api/Producto
        public IActionResult create([FromBody] ProductoJSON productoJson)
        {
            ProductoServices productoServices = new ProductoServices();
            if (productoServices.create(productoJson))
            {
                return StatusCode(201);
            }

            if (productoServices.errorCode == 120303)
            {
                return StatusCode(400, productoServices.errorMessage);
            }

            return StatusCode(500, productoServices.errorMessage);
        }
        
        [HttpGet("GetProductos")] // post api/Cliente/GeProductos
        public ActionResult getProductos([FromQuery(Name = "PageNumber")] int PageNumber,
            [FromQuery(Name = "Rows")] int Rows)
        {
            ProductoServices clienteServices = new ProductoServices();
            List<Producto> lProducto = clienteServices.ReturnProducts(PageNumber, Rows);
            double total = 0;

            string query = "SELECT COUNT(*) AS 'TOTAL' FROM [adpruebas_de_timbrado].[dbo].[admProductos];";

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

            ListOfProductos listOfProductos = new ListOfProductos(lProducto, PageNumber, Convert.ToInt32(totalPages));
            string jsonString;
            jsonString = JsonSerializer.Serialize(listOfProductos);
            return Ok(jsonString);
        }
    }
}