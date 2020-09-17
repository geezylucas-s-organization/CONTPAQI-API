using System;
using System.IO;

namespace CONTPAQ_API
{
    public abstract class ContpaqItem
    {
        public string errorMessage;
        public int errorCode;
        public void createErrorLog()
        {
            string fechaError = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
            string logPath = @"C:\logs\Error" + fechaError;
            File.Create(logPath);
            if (!File.Exists(logPath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(logPath))
                {
                    sw.WriteLine(errorMessage);
                    sw.Close();
                }	
            }
        }
    }
}