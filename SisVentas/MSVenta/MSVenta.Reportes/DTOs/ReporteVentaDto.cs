using System;
using System.Collections.Generic;

namespace MSVenta.Reportes.DTOs
{
    public class NotaVentaDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public decimal MontoTotal { get; set; }
        public string Glosa { get; set; }
        public string Estado { get; set; }
        public string TipoPago { get; set; }
        public int ClienteId { get; set; }
        public ClienteDto Cliente { get; set; }
        public List<DetalleVentaDto> Detalles { get; set; }
    }

    public class ClienteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
    }

    public class DetalleVentaDto
    {
        public int Id { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUni { get; set; }
        public decimal PrecioSubtotal { get; set; }
        public string NombreProducto { get; set; }
    }
}