namespace MSVenta.Compras.Models
{
    public class DetalleCompra
    {
        public int Id { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUni { get; set; }
        public decimal SubTotal { get; set; } = 0;
        public string NombreProducto { get; set; }

        public int CompraId { get; set; }
        public NotaCompra Compra { get; set; }

        public int? ProductoId { get; set; }
    }
}