using System;

namespace MSVenta.Venta.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public double MontoTotal { get; set; }
        public string Glosa { get; set; }
        public string Estado { get; set; }
        public string TipoPago { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public int UsuarioId { get; set; }
    }
}