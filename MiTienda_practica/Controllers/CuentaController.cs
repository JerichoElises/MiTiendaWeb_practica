using Microsoft.AspNetCore.Mvc;
using MiTienda_practica.Models;
using MiTienda_practica.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MiTienda_practica.Controllers
{
    public class CuentaController(UsuarioService _usuarioService) : Controller
    {
        public IActionResult Login()
        {
            var viewModel = new LoginVM();
            return View(viewModel);
        }

        [HttpPost]
        public async Task <IActionResult> Login(LoginVM viewmodel)
        {

            if (!ModelState.IsValid) return View(viewmodel);
            var found = await _usuarioService.Login(viewmodel);

            if (found.UsuarioId == 0)
            {
                ViewBag.MensajeExito = "No hay coincidencias";
                return View();


            }
            else
            {
                List<Claim>claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, found.UsuarioId.ToString()),
                    new Claim(ClaimTypes.Name, found.NombreCompleto),
                    new Claim(ClaimTypes.Email, found.Correo),
                    new Claim(ClaimTypes.Role, found.Rol)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties() {AllowRefresh= true, IsPersistent= true }
                    );
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Registro()
        {
            var viewModel = new UsuarioVM();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Registro(UsuarioVM viewmodel)
        {

            if (!ModelState.IsValid) return View(viewmodel);

            try
            {
                await _usuarioService.Registro(viewmodel);
                ViewBag.MensajeExito = "Tu cuenta ha sido registrada, porfavor intenta Iniciar sesión";
                ViewBag.Class = "alert-success";
            }
            catch(Exception ex)
            {
                ViewBag.MensajeExito = ex.Message;
                ViewBag.Class = "alert-success";
            }
            return View();
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}

