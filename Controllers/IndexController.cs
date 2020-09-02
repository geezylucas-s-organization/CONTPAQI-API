using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

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