using MiTienda_practica.Entities;
using MiTienda_practica.Models;
using MiTienda_practica.Repositories;
using System.Linq;

namespace MiTienda_practica.Services
{
    public class OrdenService(OrdenRepository _ordenRepository)
    {

        public async Task AddAsync(List<ArticuloCarritoVM> carritoVM, int usuarioId)
        {
            Orden orden = new Orden
            {
                FechaOrden = DateTime.Now,
                UsuarioId = usuarioId,
                TotalMonto = carritoVM.Sum(x => x.Precio * x.Quantity),
                ArticuloPedidos = carritoVM.Select(x => new ArticuloPedido
                {
                    ProductoId = x.ProductId,
                    Cantidad = x.Quantity,
                    Precio = x.Precio
                }).ToList()
            };

            await _ordenRepository.AddAsync(orden);
        }

        public async Task<List<OrdenVM>> GetAllByUserAsync(int usuarioId)
        {
            var ordenes = await _ordenRepository.GetAllWithDetailsAsync(usuarioId);
            
            var ordenesVM = ordenes.Select(x => new OrdenVM
            {
                FechaOrden = x.FechaOrden.ToString("dd/MM/yyyy"),
                MontoTotal = x.TotalMonto.ToString("C2"),
                ordenArticulos = x.ArticuloPedidos.Select(x => new OrdenArticuloVM
                {
                    NombreArticulo = x.Producto.Nombre,
                    Quantity = x.Cantidad,
                    Precio = x.Precio.ToString("C")
                }).ToList()
            }).ToList();

            return ordenesVM;
        }

    }
}
