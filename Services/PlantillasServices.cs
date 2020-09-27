using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CONTPAQ_API.Models;
using Hangfire;
using Hangfire.SqlServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CONTPAQ_API.Services
{
    public class PlantillasServices
    {
        public static void initializeHangfire()
        {
        }

        public static void func()
        {
            RecurringJob.AddOrUpdate(() => checkForPlantillas(), Cron.Minutely());

            using (var server = new BackgroundJobServer())
            {
                //Console.ReadLine();
            }
        }

        public static void checkForPlantillas()
        {
            string connString =
                @"Data Source=DESKTOP-0IF7KH8\COMPAC;Initial Catalog=Plantillas;User ID=sa;Password=Supervisor1.";
            string query = "SELECT * FROM Documentos";
            List<Plantillas> plantillas = new List<Plantillas>();
            SqlConnection connection = new SqlConnection(connString);
            SqlCommand command = new SqlCommand(query, connection);
            
            
            command.Connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    plantillas.Add(new Plantillas
                    {
                        id = Convert.ToInt32(reader.GetInt32(0)),
                        jsonFactura = reader.GetString(1),
                        ultimaVezFacturada = reader.GetDateTime(2),
                        proximaFactura = reader.GetDateTime(3),
                        estatus = reader.GetBoolean(4)
                    });
                }


                foreach (var plantilla in plantillas)
                {
                    if (!plantilla.estatus) continue;
                    if (plantilla.proximaFactura.Date == DateTime.Today)
                    {
                        int hora = plantilla.proximaFactura.Hour;
                        DocumentoServices documentoServices = new DocumentoServices();
                        Documento documento = new Documento();
                        
                        

                        BackgroundJob.Schedule(() => documentoServices.createDocumento(documento),
                            TimeSpan.FromHours(hora));
                    }
                }
                
                command.Connection.Close();
            }
        }
    }
}