using System.Collections.Generic;
using System.Text.Json;
using CONTPAQ_API.Models;
using CONTPAQ_API.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace CONTPAQ_API.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("api/[controller]")]
    public class ConceptoController : Controller
    {
        // GET
        [HttpGet("GetConcepto")] // GET api/Conceptos
        public ActionResult getConceptos()
        {
            List<Concepto> lConceptos = new ConceptoServices().returnConceptos();
            string jsonString;
            jsonString = JsonSerializer.Serialize(lConceptos);

            return Ok(jsonString);

        }

    }
}