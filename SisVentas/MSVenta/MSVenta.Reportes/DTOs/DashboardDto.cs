using System;
using System.Collections.Generic;

namespace MSVenta.Reportes.DTOs
{
    public class DashboardDto
    {
        // Ventas de hoy
        public decimal VentasHoyMonto { get; set; }
        public int VentasHoyCantidad { get; set; }

        // Inventario
        public int TotalArticulos { get; set; }
        public int TotalUnidadesEnStock { get; set; }
        public int TotalStockBajo { get; set; }
        public List<ArticuloReporteDto> AlertasStockBajo { get; set; } = new List<ArticuloReporteDto>();

        // Compras
        public int OrdenesPendientes { get; set; }

        // Últimas ventas (para la tabla del dashboard)
        public List<NotaVentaDto> UltimasVentas { get; set; } = new List<NotaVentaDto>();
    }

    public class OrdenCompraDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public string Glosa { get; set; }
        public int? ProveedorId { get; set; }
        public ProveedorDto Proveedor { get; set; }
    }
}