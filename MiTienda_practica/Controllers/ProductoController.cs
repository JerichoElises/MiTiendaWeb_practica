using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiTienda_practica.Models;
using MiTienda_practica.Services;

namespace MiTienda_practica.Controllers
{
    public class ProductoController(ProductoService _productoService) : Controller

    {
        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.GetAllAsync();
            return View(productos);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            var categorias = await _productoService.GetCategoriasAsync();

            ProductoVM model;

            if (id == 0)
            {
                model = new ProductoVM
                {
                    Categoria = new CategoriaVM()
                };
            }
            else
            {
                model = await _productoService.GetByIdAsync(id);
            }

            model.Categorias = categorias.Select(x => new SelectListItem
            {
                Value = x.CategoriaId.ToString(),
                Text = x.Nombre
            }).ToList();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddEdit(ProductoVM entityVM)
        {

            ViewBag.mensajeExito = null;
            ModelState.Remove("Categorias");
            ModelState.Remove("Categoria.Nombre");
            if (!ModelState.IsValid) return View(entityVM);

            if (entityVM.ProductoId == 0)
            {
                await _productoService.AddAsync(entityVM);
                ModelState.Clear();
                entityVM = new ProductoVM();
                ViewBag.MensajeExito = "El producto fue creado";
            }
            else
            {
                await _productoService.EditAsync(entityVM);
                ViewBag.MensajeExito = "El producto fue actualizado";
            }

            return View(entityVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _productoService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

    }
}
    
