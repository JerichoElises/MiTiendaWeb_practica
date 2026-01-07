using Microsoft.EntityFrameworkCore;
using MiTienda_practica.Context;
using MiTienda_practica.Entities;

namespace MiTienda_practica.Repositories
{
    public class OrdenRepository : GenericRepository<Orden>
    {
        private readonly AppDbContext _dbContext;
        public OrdenRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Orden orden)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                foreach (var detalle in orden.ArticuloPedidos)
                {
                    var producto = await _dbContext.Productos.FindAsync(detalle.ProductoId);
                    producto!.Stock -= detalle.Cantidad;
                }

                await _dbContext.Ordens.AddAsync(orden);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<Orden>> GetAllWithDetailsAsync(int usuarioId)
        {
            var ordenes = await _dbContext.Ordens.Where(x => x.UsuarioId == usuarioId)
                .Include(x => x.ArticuloPedidos).ThenInclude(x => x.Producto)
                .ToListAsync();
            return ordenes;
        }
    }
}
