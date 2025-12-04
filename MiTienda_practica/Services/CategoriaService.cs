using MiTienda_practica.Entities;
using MiTienda_practica.Models;
using MiTienda_practica.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MiTienda_practica.Services
{
    public class CategoriaService(GenericRepository<Categoria> _categoriaRepository)
    {
        public async Task<IEnumerable<CategoriaVM>> GetAllAsync()
        {
            var categories = await _categoriaRepository.GetAllAsync();

            var categoriesVM = categories.Select(item =>
            new CategoriaVM
            {
                CategoriaId = item.CategoriaId,
                Nombre = item.Nombre
            }
            ).ToList();

            return categoriesVM;
        }

        public async Task AddAsync(CategoriaVM viewModel)
        {
            var entity = new Categoria
            {
                Nombre = viewModel.Nombre
            };
            await _categoriaRepository.AddAsync(entity);
        }
        public async Task<CategoriaVM?> GetByIdAsync(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            var categoriaVM = new CategoriaVM();

            if (categoria != null)
            {
                categoriaVM.Nombre = categoria.Nombre;
                categoriaVM.CategoriaId = categoria.CategoriaId;

            }
            return categoriaVM;
        }

        public async Task EditAsync(CategoriaVM viewModel)
        {
            var entity = new Categoria
            {
                CategoriaId = viewModel.CategoriaId,
                Nombre = viewModel.Nombre
            };
            await _categoriaRepository.EditAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
          var cartegoria =  await _categoriaRepository.GetByIdAsync(id);
            await _categoriaRepository.DeleteAsync(cartegoria);

        }
    }
}
