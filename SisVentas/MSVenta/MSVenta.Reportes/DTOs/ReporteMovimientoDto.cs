using System;
using System.Collections.Generic;

namespace MSVenta.Reportes.DTOs
{
    public class IngresoDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Glosa { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }
        public bool SoloRegistro { get; set; }
        public List<DetalleMovimientoDto> Detalles { get; set; }
    }

    public class EgresoDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Glosa { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }
        public List<DetalleMovimientoDto> Detalles { get; set; }
    }

    public class DetalleMovimientoDto
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioCompra { get; set; }
        public string Observacion { get; set; }
        public ArticuloAlmacenDto ArticuloAlmacen { get; set; }
    }

    public class ArticuloAlmacenDto
    {
        public int Id { get; set; }
        public ArticuloDto Articulo { get; set; }
    }

    public class ArticuloDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}