using System.Buffers;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiTienda_practica.Models;
using MiTienda_practica.Services;
using MiTienda_practica.Utilities;

namespace MiTienda_practica.Controllers
{
    public class HomeController(
        CategoriaService _categoriaService,
        ProductoService _productoService
        ) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriaService.GetAllAsync();
            var productos = await _productoService.GetCatalogoAsync();

            var catalogo = new CatalogoVM
            {
                Categorias = categorias,
                Productos = productos
            };

            return View(catalogo);
        }

        public async Task<IActionResult> FilterByCategoria(int id, string nombre)
        {
            var categorias = await _categoriaService.GetAllAsync();
            var productos = await _productoService.GetCatalogoAsync(categoriaId:id);

            var catalogo = new CatalogoVM
            {
                Categorias = categorias,
                Productos = productos,
                filterBay = nombre
            };

            return View("Index",catalogo);
        }

        [HttpPost]
        public async Task<IActionResult> FilterBySearch(string value)
        {
            var categorias = await _categoriaService.GetAllAsync();
            var productos = await _productoService.GetCatalogoAsync(busqueda: value);

            var catalogo = new CatalogoVM
            {
                Categorias = categorias,
                Productos = productos,
                filterBay = $"Resultados para: {value}"
            };

            return View("Index", catalogo);
        }

        public async Task<IActionResult> ProductoDetalle(int id)  { 
            var producto = await _productoService.GetByIdAsync(id);
            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarArticuloCarrito(int productoId, int quantity)
        {
            var producto = await _productoService.GetByIdAsync(productoId);

            var carrito = HttpContext.Session.Get<List<ArticuloCarritoVM>>("Carrito") ?? new List<ArticuloCarritoVM>();

            if (carrito.Find(x => x.ProductId == productoId) == null)
            {
                carrito.Add(new ArticuloCarritoVM
                {
                    ProductId = productoId,
                    ImagenNombre = producto.ImagenNombre,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Quantity = quantity
                });
            }
            else 
            {
                var ActualizarProducto = carrito.Find(x => x.ProductId == productoId);
                ActualizarProducto!.Quantity += quantity;
            }

            HttpContext.Session.Set("Carrito", carrito);
            ViewBag.message = "Producto agregado al carrito exitosamente.";
            return View("ProductoDetalle", producto);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
