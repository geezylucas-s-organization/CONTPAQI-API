using CONTPAQ_API.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

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
    }
}