namespace MiTienda_practica.Models
{
    public class CatalogoVM
    {
        public IEnumerable <CategoriaVM> Categorias { get; set; }
        public IEnumerable<ProductoVM> Productos { get; set; }
        public string filterBay { get; set; }

    }
}
