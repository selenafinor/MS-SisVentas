using System;
using System.Collections.Generic;

namespace MSVenta.Venta.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public decimal MontoTotal { get; set; }
        public string Glosa { get; set; }
        public string Estado { get; set; } = "activo";
        public string TipoPago { get; set; } = "contado";
        public bool PagoConfirmado { get; set; } = false;
        public string IdTransaccionQr { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public int UsuarioId { get; set; }
        public ICollection<DetalleVenta> Detalles { get; set; }
    }
}