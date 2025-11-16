using Microsoft.EntityFrameworkCore;
using MiTienda_practica.Entities;

namespace MiTienda_practica.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Orden> Ordens { get; set; }
        public DbSet<ArticuloPedido> ArticuloPedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Categoria>(e =>
            {
                e.HasKey("CategoriaId");
                e.Property("CategoriaId").ValueGeneratedOnAdd();
                e.HasData(
                    new Categoria { CategoriaId = 1, Nombre = "Ropa" },
                    new Categoria { CategoriaId = 2, Nombre = "Electrónica" },
                    new Categoria { CategoriaId = 3, Nombre = "Hogar" }
                );
            });
            modelBuilder.Entity<Producto>((e =>
            {
                e.HasKey("ProductoId");
                e.Property("ProductoId").ValueGeneratedOnAdd();
                e.Property("Precio").HasColumnType("decimal(10,2)");
                e.HasOne(e => e.Categoria).WithMany(p => p.Productos).HasForeignKey(e => e.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            }));
            modelBuilder.Entity<Usuario>((e =>
            {
                e.HasKey("UsuarioId");
                e.Property("UsuarioId").ValueGeneratedOnAdd();
            }));
            modelBuilder.Entity<Orden>((e =>
            {
                e.HasKey("OrdenId");
                e.Property("OrdenId").ValueGeneratedOnAdd();
                e.Property("TotalMonto").HasColumnType("decimal(10,2)");
                e.HasOne(e => e.Usuario).WithMany(p => p.Ordenes).HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            }));
            modelBuilder.Entity<ArticuloPedido>((e =>
            {
                e.HasKey("ArticuloPedidoId");
                e.Property("ArticuloPedidoId").ValueGeneratedOnAdd();
                e.Property("Precio").HasColumnType("decimal(10,2)");
                e.HasOne(e => e.Orden).WithMany(p => p.ArticuloPedidos).HasForeignKey(e => e.OrdenId)
                .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(e => e.Producto).WithMany().HasForeignKey(e => e.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            }));
        }


    }
}
