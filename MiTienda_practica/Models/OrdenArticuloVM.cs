using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace MiTienda_practica.Models
{
    public class OrdenArticuloVM
    {
        public string NombreArticulo { get; set; }
        public int Quantity { get; set; }
        public string Precio { get; set; }

    }
}
