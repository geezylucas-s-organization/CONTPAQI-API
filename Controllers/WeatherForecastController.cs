using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            string rutaBinarios = @"C:\Program Files (x86)\Compac\COMERCIAL";
            string nombrePAQ = "CONTPAQ I COMERCIAL";
            string rutaEmpresa = @"C:\Compac\Empresas\adprueba";
            int lError = 0;

            //Paso 1: Pasar la ruta de los binarios, ya que ahí se encuentra la dll a consumir
            SDK.SetCurrentDirectory(rutaBinarios);
            SDK.fInicioSesionSDK("SUPERVISOR", "");

            //Paso 2: Pasar el nombre del sistema con el cual vamos a trabajar
            lError = SDK.fSetNombrePAQ(nombrePAQ);
            if (lError != 0)
            {
                SDK.rError(lError);
            }
            else
            {
                //Paso 3: Indicar la ruta de la empresa a utilzar.
                lError = SDK.fAbreEmpresa(rutaEmpresa);
                if (lError != 0)
                {
                    SDK.rError(lError);
                }
                else
                {
                    Console.WriteLine("Empresa abierta.");
                }
            }

            //Pasos complementarios
            SDK.fCierraEmpresa();
            SDK.fTerminaSDK();

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }
    }
}