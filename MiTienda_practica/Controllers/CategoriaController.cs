using Microsoft.AspNetCore.Mvc;
using MiTienda_practica.Context;
using MiTienda_practica.Entities;
using MiTienda_practica.Models;

namespace MiTienda_practica.Controllers
{
    public class CategoriaController(AppDbContext _dbContext) : Controller
    {
        public IActionResult Index()
        {
            var categorias = _dbContext.Categorias.Select(item =>
            new CategoriaVM
            {
                CategoriaId = item.CategoriaId,
                Nombre = item.Nombre
            }
            ).ToList();
            return View(categorias);
        }
            
    }
}
