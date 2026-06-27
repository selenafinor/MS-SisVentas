using System.Collections.Generic;
namespace MSVenta.Inventario.Models
{
    public class AdquisicionRegistradaEvento
    {
        public int AdquisicionId { get; set; }
        public int UsuarioId { get; set; }
        public List<DetalleAdquisicionEvento> Detalles { get; set; }
    }
    public class DetalleAdquisicionEvento
    {
        public int IdProducto { get; set; }
        public int IdAlmacen { get; set; }
        public string NombreProducto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUni { get; set; }
    }
}