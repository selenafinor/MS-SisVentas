using System;
using System.Linq;
using System.Threading.Tasks;
using MSVenta.Reportes.DTOs;

namespace MSVenta.Reportes.Services
{
    public class DashboardService
    {
        private readonly VentaService _ventaService;
        private readonly InventarioService _inventarioService;
        private readonly CompraService _compraService;

        public DashboardService(
            VentaService ventaService,
            InventarioService inventarioService,
            CompraService compraService)
        {
            _ventaService = ventaService;
            _inventarioService = inventarioService;
            _compraService = compraService;
        }

        public async Task<DashboardDto> GetDashboardAsync()
        {
            // Se piden en paralelo porque son 3 llamadas HTTP independientes a otros microservicios.
            var ventasTask = _ventaService.GetNotasVentaAsync();
            var articulosTask = _inventarioService.GetArticulosAsync();
            var ordenesTask = _compraService.GetOrdenesCompraAsync();

            await Task.WhenAll(ventasTask, articulosTask, ordenesTask);

            var ventas = ventasTask.Result;
            var articulos = articulosTask.Result;
            var ordenes = ordenesTask.Result;

            var hoy = DateTime.Today;

            var ventasHoy = ventas
                .Where(v => v.Fecha.Date == hoy && (v.Estado == "activo" || v.Estado == "pagado"))
                .ToList();

            var stockBajo = articulos
                .Where(a => a.Stock <= a.StockMin)
                .OrderBy(a => a.Stock)
                .ToList();

            var ordenesPendientes = ordenes.Count(o => o.Estado == "pendiente");

            var ultimasVentas = ventas
                .OrderByDescending(v => v.Fecha)
                .ThenByDescending(v => v.Hora)
                .Take(5)
                .ToList();

            return new DashboardDto
            {
                VentasHoyMonto = ventasHoy.Sum(v => v.MontoTotal),
                VentasHoyCantidad = ventasHoy.Count,

                TotalArticulos = articulos.Count,
                TotalUnidadesEnStock = articulos.Sum(a => a.Stock),
                TotalStockBajo = stockBajo.Count,
                AlertasStockBajo = stockBajo,

                OrdenesPendientes = ordenesPendientes,

                UltimasVentas = ultimasVentas
            };
        }
    }
}