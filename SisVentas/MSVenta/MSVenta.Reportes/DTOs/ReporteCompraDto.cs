using System;

namespace MSVenta.Reportes.DTOs
{
    public class NotaCompraDto
    {
        public int Id { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal TotalCompra { get; set; }
        public string Estado { get; set; }
        public string Glosa { get; set; }
        public string TipoPago { get; set; }
        public int? ProveedorId { get; set; }
        public ProveedorDto Proveedor { get; set; }
    }

    public class ProveedorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class DetalleCompraDto
    {
        public int Id { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUni { get; set; }
        public decimal SubTotal { get; set; }
    }
}