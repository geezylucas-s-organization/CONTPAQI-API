using System;
using System.IO;

namespace CONTPAQ_API.Services
{
    public class SDKServices
    {
        public static bool Conectar()
        {
            try
            {
                string rutaBinarios = @"C:\Program Files (x86)\Compac\COMERCIAL";
                string nombrePAQ = "CONTPAQ I COMERCIAL";
                string rutaEmpresa = @"C:\Compac\Empresas\adpruebas_de_timbrado";
                int lError;

                //Paso 1: Pasar la ruta de los binarios, ya que ahí se encuentra la dll a consumir
                SDK.SetCurrentDirectory(rutaBinarios);
                SDK.fInicioSesionSDK("SUPERVISOR", "");

                //Paso 2: Pasar el nombre del sistema con el cual vamos a trabajar
                lError = SDK.fSetNombrePAQ(nombrePAQ);
                if (lError != 0)
                {
                    return false;
                }

                //Paso 3: Indicar la ruta de la empresa a utilzar.
                lError = SDK.fAbreEmpresa(rutaEmpresa);

                return lError == 0;
            }
            catch (Exception ex)
            {
                string path = @"c:\temp\MyTest.txt";
                if (!System.IO.File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = System.IO.File.CreateText(path))
                    {
                        sw.WriteLine(ex.Message);
                    }
                }

                return false;
            }
        }

        public static bool Termina()
        {
            try
            {
                SDK.fCierraEmpresa();
                SDK.fTerminaSDK();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}