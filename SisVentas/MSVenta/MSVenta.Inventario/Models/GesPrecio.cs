using System;
namespace MSVenta.Inventario.Models
{
    public class GesPrecio
    {
        public int Id { get; set; }
        public int Id_Articulo { get; set; }
        public Articulo Articulo { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Today;
        public string MetodoInventario { get; set; } = "PROMEDIO";
    }
}