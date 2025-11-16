namespace MiTienda_practica.Entities
{
    public class ArticuloPedido
    {
        public int ArticuloPedidoId { get; set; }
        public int OrdenId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }


        public Orden? Orden { get; set; }
        public Producto? Producto { get; set; }
    }
}
