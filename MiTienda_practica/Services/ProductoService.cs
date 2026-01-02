using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using MiTienda_practica.Entities;
using MiTienda_practica.Models;
using MiTienda_practica.Repositories;
using System.Linq.Expressions;

namespace MiTienda_practica.Services
{
    public class ProductoService(

        GenericRepository<Categoria> _categoriaRepository,
        GenericRepository<Producto> _productoRepository,
        IWebHostEnvironment webHostEnvironment
    )
    {

        public async Task<IEnumerable<ProductoVM>> GetAllAsync()
        {
            var productos = await _productoRepository.GetAllAsync(
                includes: new Expression<Func<Producto, object>>[]
                {
                    x => x.Categoria!
                });

            var productosVM = productos.Select(item => new ProductoVM
            {
                ProductoId = item.ProductoId,
                Categoria = new CategoriaVM
                {
                    CategoriaId = item.Categoria!.CategoriaId,
                    Nombre = item.Categoria.Nombre
                },
                Nombre = item.Nombre,
                Descripcion = item.Descripcion,
                Precio = item.Precio,
                Stock = item.Stock,
                ImagenNombre = item.ImagenNombre
            }).ToList();

            return productosVM;

        }

        public async Task<ProductoVM> GetByIdAsync(int id)
        {

            var producto = await _productoRepository.GetByIdAsync(id);

            var categorias = await _categoriaRepository.GetAllAsync();

            var productoVM = new ProductoVM();

            if (producto != null)
            {
                productoVM = new ProductoVM
                {
                    ProductoId = producto.ProductoId,
                    Categoria = new CategoriaVM
                    {
                        CategoriaId = producto.Categoria!.CategoriaId,
                        Nombre = producto.Categoria.Nombre
                    },
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    ImagenNombre = producto.ImagenNombre
                };

            }

            productoVM.Categorias = categorias.Select(item => new SelectListItem
            {
                Value = item.CategoriaId.ToString(),
                Text = item.Nombre
            }).ToList();

            return productoVM;
        }

        public async Task<List<CategoriaVM>> GetCategoriasAsync()
        {
            var categorias = await _categoriaRepository.GetAllAsync();

            return categorias.Select(item => new CategoriaVM
            {
                CategoriaId = item.CategoriaId,
                Nombre = item.Nombre
            }).ToList();
        }


        public async Task AddAsync(ProductoVM viewModel)
        {
            string? uniqueFileName = null;

            // 1. Si viene imagen, guardarla
            if (viewModel.ImagenArchivo != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "imagenes");

                // Crear carpeta si no existe
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.ImagenArchivo.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.ImagenArchivo.CopyToAsync(fileStream);
                }

                viewModel.ImagenNombre = uniqueFileName;
            }

            // 2. Crear el producto SIEMPRE (con o sin imagen)
            var entity = new Producto
            {
                CategoriaId = viewModel.Categoria.CategoriaId,
                Nombre = viewModel.Nombre,
                Descripcion = viewModel.Descripcion,
                Precio = viewModel.Precio,
                Stock = viewModel.Stock,
                ImagenNombre = viewModel.ImagenNombre
            };

            await _productoRepository.AddAsync(entity);
        }

        public async Task EditAsync(ProductoVM viewModel)
        {

            var producto = await _productoRepository.GetByIdAsync(viewModel.ProductoId);

            if (viewModel.ImagenArchivo != null)
            {

                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "imagenes");
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.ImagenArchivo.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                    await viewModel.ImagenArchivo.CopyToAsync(fileStream);

                // si EXISTÍA imagen anterior, borrarla
                if (!string.IsNullOrEmpty(producto.ImagenNombre))
                {
                    var previousImage = producto.ImagenNombre;
                    string deleteFilePath = Path.Combine(uploadsFolder, previousImage);

                    if (File.Exists(deleteFilePath))
                        File.Delete(deleteFilePath);
                }


                viewModel.ImagenNombre = uniqueFileName;
            }
            else
            {
                viewModel.ImagenNombre = producto.ImagenNombre;
            }

            producto.CategoriaId = viewModel.Categoria.CategoriaId;
            producto.Nombre = viewModel.Nombre;
            producto.Descripcion = viewModel.Descripcion;
            producto.Precio = viewModel.Precio;
            producto.Stock = viewModel.Stock;
            producto.ImagenNombre = viewModel.ImagenNombre;

            await _productoRepository.EditAsync(producto);

        }

        public async Task DeleteAsync(int id)
        {

            var producto = await _productoRepository.GetByIdAsync(id);
            await _productoRepository.DeleteAsync(producto);
        }

        public async Task<IEnumerable<ProductoVM>> GetCatalogoAsync(int categoriaId = 0, string? busqueda = null)
        {
            var conditions = new List<Expression<Func<Producto, bool>>>
            {
                x => x.Stock > 0
            }; 

            if(categoriaId != 0) conditions.Add(x => x.CategoriaId == categoriaId);
            if(!string.IsNullOrEmpty(busqueda)) conditions.Add(x => x.Nombre!.Contains(busqueda) || x.Descripcion!.Contains(busqueda));

            var productos = await _productoRepository.GetAllAsync(conditions: conditions.ToArray());

            var productosVM = productos.Select(item => new ProductoVM
            {
                ProductoId = item.ProductoId,
                Nombre = item.Nombre,
                Descripcion = item.Descripcion,
                Precio = item.Precio,
                Stock = item.Stock,
                ImagenNombre = item.ImagenNombre
            }).ToList();

            return productosVM;

        }


    }

}

