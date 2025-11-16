using System.ComponentModel.DataAnnotations;

namespace MiTienda_practica.Entities
{
    public class Usuario
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

        public ICollection<Orden> Ordenes { get; set; }
    }
}
