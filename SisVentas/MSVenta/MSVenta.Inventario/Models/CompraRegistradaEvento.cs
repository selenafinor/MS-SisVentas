using System.Collections.Generic;

namespace MSVenta.Inventario.Models
{
    public class CompraRegistradaEvento
    {
        public int CompraId { get; set; }
        public int UsuarioId { get; set; }
        public List<DetalleCompraEvento> Detalles { get; set; }
    }

    public class DetalleCompraEvento
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUni { get; set; }
    }
}