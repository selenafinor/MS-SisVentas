using System;
using System.Collections.Generic;

namespace MSVenta.Compras.Models
{
    public class NotaCompra
    {
        public int Id { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal TotalCompra { get; set; } = 0;
        public string Estado { get; set; } = "activo";
        public string Glosa { get; set; }
        public string TipoPago { get; set; } = "efectivo";

        public int? ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }

        public int? UsuarioId { get; set; }

        public ICollection<DetalleCompra> Detalles { get; set; }
    }
}