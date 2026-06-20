using System.Collections.Generic;

namespace MSVenta.Venta.Models
{
    public class VentaCreadaEvento
    {
        public int VentaId { get; set; }
        public int UsuarioId { get; set; }
        public List<DetalleVentaEvento> Detalles { get; set; }
    }

    public class DetalleVentaEvento
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int IdAlmacen { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUni { get; set; }
    }
}