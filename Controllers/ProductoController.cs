using System;
using CONTPAQ_API.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

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
    }
}