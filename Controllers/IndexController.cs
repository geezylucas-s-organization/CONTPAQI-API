using System.IO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CONTPAQ_API.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class IndexController : Controller
    {
        // GET
        [HttpGet]
        public ActionResult Index()
        {
            return Ok("Ok");
        }
    }
}