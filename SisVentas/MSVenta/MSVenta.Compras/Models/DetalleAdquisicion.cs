namespace MSVenta.Compras.Models
{
    public class DetalleAdquisicion
    {
        public int Id { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUni { get; set; }
        public decimal SubTotal { get; set; } = 0;
        public string NombreProducto { get; set; }
        public int AdquisicionId { get; set; }
        public Adquisicion Adquisicion { get; set; }
        public int? ProductoId { get; set; }
        public int? AlmacenId { get; set; }
    }
}