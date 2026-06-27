namespace MSVenta.Compras.Models
{
    public class DetalleOrdenCompra
    {
        public int Id { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUni { get; set; }
        public decimal SubTotal { get; set; } = 0;
        public string NombreProducto { get; set; }
        public int OrdenId { get; set; }
        public OrdenCompra Orden { get; set; }
        public int? ProductoId { get; set; }
    }
}