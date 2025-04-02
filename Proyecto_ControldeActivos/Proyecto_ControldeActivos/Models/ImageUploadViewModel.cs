using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_ControldeActivos.Models
{

    public class FileUploadActivoViewModel
    {
        public int IdActivo { get; set; }
        public string FileB64 {  get; set; }
        public string FileExt { get; set; }
        public string FileName { get; set; }
        public string FileMimeType { get; set; }
        public int FileSize { get; set; }

    }
}