using Microsoft.AspNetCore.Mvc;
using MiTienda_practica.Services;
using System.Threading.Tasks;

namespace MiTienda_practica.Controllers
{
    public class UsuarioController(OrdenService _ordenService) : Controller
    {
        public async Task<IActionResult> MisOrdenes()
        {
            var usuarioId = 1; // Simulación de usuario autenticado con ID 1
            var ordenesvm = await _ordenService.GetAllByUserAsync(usuarioId);
            return View(ordenesvm);
        }
    }
}
