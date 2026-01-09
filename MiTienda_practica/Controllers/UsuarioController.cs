using Microsoft.AspNetCore.Mvc;
using MiTienda_practica.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MiTienda_practica.Controllers
{
    [Authorize]
    public class UsuarioController(OrdenService _ordenService) : Controller
    {
        public async Task<IActionResult> MisOrdenes()
        {
            var usuarioId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value; 
            var ordenesvm = await _ordenService.GetAllByUserAsync(int.Parse(usuarioId));
            return View(ordenesvm);
        }
    }
}
