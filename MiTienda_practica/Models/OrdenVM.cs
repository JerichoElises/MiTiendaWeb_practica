namespace MiTienda_practica.Models
{
    public class OrdenVM
    {
        public string FechaOrden { get; set; }
        public string MontoTotal { get; set; }
        public ICollection<OrdenArticuloVM> ordenArticulos { get; set; }
    }
}
