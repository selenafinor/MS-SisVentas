using System;

namespace MSVenta.Venta.Models
{
    public class DetalleVenta
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUni { get; set; }
        public double PrecioSubtotal { get; set; }
        public int VentaId { get; set; }
        public Venta Venta { get; set; }
        public int Id_Producto { get; set; }
        public int Id_Almacen { get; set; }
    }
}