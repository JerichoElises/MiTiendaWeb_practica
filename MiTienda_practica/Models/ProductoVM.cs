using Microsoft.AspNetCore.Mvc.Rendering;
using MiTienda_practica.Models;
using System.ComponentModel.DataAnnotations;

public class ProductoVM
{
    public int ProductoId { get; set; }


    public CategoriaVM Categoria { get; set; } = new CategoriaVM();


    public List<SelectListItem> Categorias { get; set; } = new List<SelectListItem>();

    [Required]
    public string Nombre { get; set; }

    [Required]
    public string Descripcion { get; set; }

    [Required]
    public decimal Precio { get; set; }

    [Required]
    public int Stock { get; set; }

    public string? ImagenNombre { get; set; } = null;
    public IFormFile? ImagenArchivo { get; set; }
}
