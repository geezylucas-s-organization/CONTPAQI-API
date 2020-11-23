using System.Collections.Generic;
using CONTPAQ_API.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
            
            string jsonString;
            jsonString = JsonSerializer.Serialize(lCliente);
            return Ok(jsonString);
        }
    }
}