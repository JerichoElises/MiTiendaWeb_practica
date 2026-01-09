using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MiTienda_practica.Entities;
using MiTienda_practica.Models;
using MiTienda_practica.Repositories;
using System.Linq.Expressions;

namespace MiTienda_practica.Services
{
    public class UsuarioService(GenericRepository<Usuario> _usuarioRepository)
    {
        public async Task<UsuarioVM> Login(LoginVM loginVM)
        {
            var conditions = new List<Expression<Func<Usuario, bool>>>()
            {
                x => x.Correo == loginVM.Correo,
                x => x.Contrasena == loginVM.Contrasena,
            };

            var found = await _usuarioRepository.GetByFilter(conditions: conditions.ToArray());

            var usuarioVM = new UsuarioVM();

            if (found != null)
            {
                usuarioVM.UsuarioId = found.UsuarioId;
                usuarioVM.NombreCompleto = found.NombreCompleto;
                usuarioVM.Correo = found.Correo;
                usuarioVM.Rol = found.Rol;
            }

            return usuarioVM;
        }

        public async Task Registro(UsuarioVM usuarioVM)
        {
            if(usuarioVM.Contrasena!= usuarioVM.RepiteContrasena)
                throw new InvalidOperationException("Las contraseñas no son iguales");

            var conditions = new List<Expression<Func<Usuario, bool>>>()
            {
                x => x.Correo == usuarioVM.Correo,
            };

            var foundCorreo = await _usuarioRepository.GetByFilter(conditions:conditions.ToArray());

            if (foundCorreo != null)
                throw new InvalidOperationException("El correo ya ha sido registrado");

            var entity = new Usuario()
            {
                NombreCompleto = usuarioVM.NombreCompleto,
                Correo = usuarioVM.Correo,
                Rol = usuarioVM.Rol,
                Contrasena = usuarioVM.Contrasena,
            }; 

            await _usuarioRepository.AddAsync(entity);
        }
    }
}
