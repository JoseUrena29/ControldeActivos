// Models/LoginModel.cs
using System.ComponentModel.DataAnnotations;

namespace Proyecto_ControldeActivos.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Usuario")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
    }
}
