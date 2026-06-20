namespace MSVenta.Compras.Models
{
    public class CatalogoProveedor
    {
        public int Id { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal StockDisponible { get; set; } = 0;
        public string Estado { get; set; } = "activo";
        public string NombreProducto { get; set; }

        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }

        public int? ProductoId { get; set; }
    }
}