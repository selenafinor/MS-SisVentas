using System;
using System.Collections.Generic;
namespace MSVenta.Compras.Models
{
    public class Adquisicion
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Today;
        public string Estado { get; set; } = "activo";
        public string Glosa { get; set; }
        public int? OrdenId { get; set; }
        public OrdenCompra Orden { get; set; }
        public int? ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }
        public int? UsuarioId { get; set; }
        public ICollection<DetalleAdquisicion> Detalles { get; set; }
    }
}