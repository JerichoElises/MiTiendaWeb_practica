using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace MiTienda_practica.Models
{
    public class CategoriaVM
    {
        public int CategoriaId { get; set; }

        [Required]
        public string Nombre { get; set; }
    }
}


