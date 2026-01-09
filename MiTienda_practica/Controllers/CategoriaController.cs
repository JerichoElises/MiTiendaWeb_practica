using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiTienda_practica.Models;
using MiTienda_practica.Services;

namespace MiTienda_practica.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriaController(CategoriaService _categoriaService) : Controller
    {
       
        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriaService.GetAllAsync();
            return View(categorias);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            if (id == 0)
            {
                return View(new CategoriaVM());
            }

            var categoriaVM = await _categoriaService.GetByIdAsync(id);
            return View(categoriaVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(CategoriaVM entityVM)
        {
            ViewBag.MensajeExito = null;

            if (!ModelState.IsValid) return View(entityVM);

            if (entityVM.CategoriaId == 0)
            {
                await _categoriaService.AddAsync(entityVM);
                ViewBag.MensajeExito = "La categoría fue creada";
                ModelState.Clear();
                entityVM = new CategoriaVM();
            }
            else
            {
                await _categoriaService.EditAsync(entityVM);
                ViewBag.MensajeExito = "La categoría fue actualizada";
            }

            return View(entityVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _categoriaService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

    }
}
