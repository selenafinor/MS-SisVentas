using System.ComponentModel.DataAnnotations.Schema;

namespace MSVenta.Venta.Models
{
    public class DetalleVenta
    {
        public int Id { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUni { get; set; }
        public decimal PrecioSubtotal { get; set; }
        public int VentaId { get; set; }
        public Venta Venta { get; set; }
        public int Id_Producto { get; set; }
        public string NombreProducto { get; set; }
        public int Id_Almacen { get; set; }
        public string NombreAlmacen { get; set; }
    }
}