using System.Collections.ObjectModel;

namespace MiTienda_practica.Entities
{
    public class Orden
    {
        public int OrdenId { get; set; }
        public DateTime FechaOrden { get; set; }
        public int UsuarioId { get; set; }
        public decimal TotalMonto { get; set; }

        public Usuario? Usuario { get; set; }
        public ICollection<ArticuloPedido> ArticuloPedidos { get; set; }

        
    }
}
