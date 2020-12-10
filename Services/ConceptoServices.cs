using System;
using System.Collections.Generic;
using CONTPAQ_API.Models;
using Microsoft.Data.SqlClient;

namespace CONTPAQ_API.Services
{
    public class ConceptoServices
    {
        public List<Concepto> returnConceptos()
        {
            List<Concepto> lConcepto = new List<Concepto>();
            string query =
                "SELECT CCODIGOCONCEPTO, CNOMBRECONCEPTO, CNOFOLIO FROM [adpruebas_de_timbrado].[dbo].[admConceptos] WHERE CCODIGOCONCEPTO = 5;";

            string connString = DatabaseServices.GetConnString();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Concepto concepto = new Concepto(Convert.ToInt32(reader.GetString(0)), reader.GetString(1).Trim(),
                            Convert.ToInt32(reader.GetDouble(2)));
                        lConcepto.Add(concepto);
                    }
                }

                return lConcepto;
            }
        }

    }
}