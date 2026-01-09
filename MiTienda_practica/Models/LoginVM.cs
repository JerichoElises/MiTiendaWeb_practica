using System.ComponentModel.DataAnnotations;

namespace MiTienda_practica.Models
{
    public class LoginVM
    {
        [Required]
        public string Correo {  get; set; }
        [Required]
        public string Contrasena { get; set; }
    }
}
