using System;
using System.Collections.Generic;
namespace MSVenta.Compras.Models
{
    public class OrdenCompra
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Today;
        public string Estado { get; set; } = "pendiente";
        public string Glosa { get; set; }
        public decimal Total { get; set; } = 0;
        public int? ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }
        public int? UsuarioId { get; set; }
        public ICollection<DetalleOrdenCompra> Detalles { get; set; }
    }
}