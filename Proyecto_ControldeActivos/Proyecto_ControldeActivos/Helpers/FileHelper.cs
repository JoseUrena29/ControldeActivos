using System;
using System.IO;

namespace Proyecto_ControldeActivos.Helpers
{
    public class FileHelper
    {
        public static void SaveBase64ToFile(string base64String, string filePath)
        {
            try
            {
                // Remove the Base64 header if present (e.g., data:image/png;base64,)
                if (base64String.StartsWith("data:image"))
                {
                    base64String = base64String.Substring(base64String.IndexOf(',') + 1);
                }

                byte[] fileBytes = Convert.FromBase64String(base64String);

                // Write the byte array to a file
                File.WriteAllBytes(filePath, fileBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static void DeleteFile(string filePath)
        {
            try
            {
                // Write the byte array to a file
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}