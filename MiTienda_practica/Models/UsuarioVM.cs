using System.ComponentModel.DataAnnotations;

namespace MiTienda_practica.Models
{
    public class UsuarioVM
    {
        public int UsuarioId { get; set; }
        [Required]
        public string NombreCompleto { get; set; }
        [Required]
        public string Correo { get; set; }
        [Required]
        public string Contrasena { get; set; }

        [Required]
        public string Rol { get; set; }

        [Required]
        public string RepiteContrasena { get; set; }

    }
}
