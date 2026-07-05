using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MSVenta.Reportes.DTOs;
using MSVenta.Reportes.Services;


namespace MSVenta.Reportes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReporteController : ControllerBase
    {
        private readonly VentaService _ventaService;
        private readonly CompraService _compraService;
        private readonly InventarioService _inventarioService;
        private readonly CorreoService _correoService;
        private readonly PdfService _pdfService;
        private readonly DashboardService _dashboardService;

        public ReporteController(
            VentaService ventaService,
            CompraService compraService,
            InventarioService inventarioService,
            CorreoService correoService,
            PdfService pdfService,
            DashboardService dashboardService)
        {
            _ventaService = ventaService;
            _compraService = compraService;
            _inventarioService = inventarioService;
            _correoService = correoService;
            _pdfService = pdfService;
            _dashboardService = dashboardService;
        }

        // ─── DASHBOARD ────────────────────────────────────────

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                var dashboard = await _dashboardService.GetDashboardAsync();
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // ─── VENTAS ───────────────────────────────────────────

        [HttpGet("ventas")]
        public async Task<IActionResult> GetReporteVentas(
    [FromQuery] string desde = null,
    [FromQuery] string hasta = null,
    [FromQuery] string estado = null,
    [FromQuery] string tipoPago = null)
        {
            try
            {
                var ventas = await _ventaService.GetNotasVentaAsync();

                if (!string.IsNullOrEmpty(desde))
                {
                    var fechaDesde = DateTime.Parse(desde);
                    ventas = ventas.FindAll(v => v.Fecha >= fechaDesde);
                }
                if (!string.IsNullOrEmpty(hasta))
                {
                    var fechaHasta = DateTime.Parse(hasta);
                    ventas = ventas.FindAll(v => v.Fecha <= fechaHasta);
                }
                if (!string.IsNullOrEmpty(estado) && estado != "todos")
                    ventas = ventas.FindAll(v => v.Estado == estado);

                if (!string.IsNullOrEmpty(tipoPago) && tipoPago != "todos")
                    ventas = ventas.FindAll(v => v.TipoPago == tipoPago);

                var totalVendido = 0m;
                var ventasCanceladas = 0;

                foreach (var v in ventas)
                {
                    if (v.Estado == "activo" || v.Estado == "pagado")
                        totalVendido += v.MontoTotal;
                    if (v.Estado == "cancelado")
                        ventasCanceladas++;
                }

                return Ok(new
                {
                    totalVendido,
                    ventasCanceladas,
                    totalRegistros = ventas.Count,
                    ventas
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // ─── COMPRAS ──────────────────────────────────────────

        [HttpGet("compras")]
        public async Task<IActionResult> GetReporteCompras(
    [FromQuery] string desde = null,
    [FromQuery] string hasta = null,
    [FromQuery] string estado = null)
        {
            try
            {
                var compras = await _compraService.GetNotasCompraAsync();

                if (!string.IsNullOrEmpty(desde))
                {
                    var fechaDesde = DateTime.Parse(desde);
                    compras = compras.FindAll(c => c.FechaCompra >= fechaDesde);
                }
                if (!string.IsNullOrEmpty(hasta))
                {
                    var fechaHasta = DateTime.Parse(hasta);
                    compras = compras.FindAll(c => c.FechaCompra <= fechaHasta);
                }
                if (!string.IsNullOrEmpty(estado) && estado != "todos")
                    compras = compras.FindAll(c => c.Estado == estado);

                var totalComprado = 0m;
                foreach (var c in compras)
                    totalComprado += c.TotalCompra;

                return Ok(new
                {
                    totalComprado,
                    totalRegistros = compras.Count,
                    compras
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // ─── INVENTARIO ───────────────────────────────────────

        [HttpGet("inventario")]
        public async Task<IActionResult> GetReporteInventario()
        {
            try
            {
                var articulos = await _inventarioService.GetArticulosAsync();

                var stockBajo = articulos.FindAll(a => a.Stock <= a.StockMin);
                var totalArticulos = articulos.Count;

                return Ok(new
                {
                    totalArticulos,
                    totalStockBajo = stockBajo.Count,
                    articulos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }
        // ─── MOVIMIENTOS ──────────────────────────────────────

        [HttpGet("movimientos")]
        public async Task<IActionResult> GetReporteMovimientos(
    [FromQuery] string desde = null,
    [FromQuery] string hasta = null,
    [FromQuery] string tipo = null)
        {
            try
            {
                var ingresos = await _inventarioService.GetIngresosAsync(incluirDetalle: true);
                var egresos = await _inventarioService.GetEgresosAsync(incluirDetalle: true);

                if (!string.IsNullOrEmpty(desde))
                {
                    var fechaDesde = DateTime.Parse(desde);
                    ingresos = ingresos.FindAll(i => i.Fecha >= fechaDesde);
                    egresos = egresos.FindAll(e => e.Fecha >= fechaDesde);
                }
                if (!string.IsNullOrEmpty(hasta))
                {
                    var fechaHasta = DateTime.Parse(hasta);
                    ingresos = ingresos.FindAll(i => i.Fecha <= fechaHasta);
                    egresos = egresos.FindAll(e => e.Fecha <= fechaHasta);
                }

                if (tipo == "ingreso")
                    egresos = new System.Collections.Generic.List<EgresoDto>();
                else if (tipo == "egreso")
                    ingresos = new System.Collections.Generic.List<IngresoDto>();

                return Ok(new
                {
                    totalIngresos = ingresos.Count,
                    totalEgresos = egresos.Count,
                    ingresos,
                    egresos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // ─── ENVIAR POR CORREO ────────────────────────────────

        [HttpPost("enviar-correo")]
        public async Task<IActionResult> EnviarReportePorCorreo(
    [FromBody] EnviarCorreoRequest request)
        {
            try
            {
                if (request.Destinatarios == null || request.Destinatarios.Count == 0)
                    return BadRequest(new { mensaje = "Debe indicar al menos un destinatario" });

                byte[] pdf;
                string nombreArchivo;

                switch (request.TipoReporte?.ToLower())
                {
                    case "ventas":
                        var ventas = await _ventaService.GetNotasVentaAsync();
                        var totalVendido = 0m;
                        var canceladas = 0;
                        foreach (var v in ventas)
                        {
                            if (v.Estado == "activo" || v.Estado == "pagado") totalVendido += v.MontoTotal;
                            if (v.Estado == "cancelado") canceladas++;
                        }
                        pdf = _pdfService.GenerarReporteVentas(ventas, totalVendido, canceladas);
                        nombreArchivo = "reporte-ventas.pdf";
                        break;

                    case "compras":
                        var compras = await _compraService.GetNotasCompraAsync();
                        var totalComprado = 0m;
                        foreach (var c in compras) totalComprado += c.TotalCompra;
                        pdf = _pdfService.GenerarReporteCompras(compras, totalComprado);
                        nombreArchivo = "reporte-compras.pdf";
                        break;

                    case "inventario":
                        var articulos = await _inventarioService.GetArticulosAsync();
                        pdf = _pdfService.GenerarReporteInventario(articulos);
                        nombreArchivo = "reporte-inventario.pdf";
                        break;

                    case "movimientos":
                        var ingresos = await _inventarioService.GetIngresosAsync(incluirDetalle: true);
                        var egresos = await _inventarioService.GetEgresosAsync(incluirDetalle: true);
                        pdf = _pdfService.GenerarReporteMovimientos(ingresos, egresos);
                        nombreArchivo = "reporte-movimientos.pdf";
                        break;

                    default:
                        return BadRequest(new { mensaje = "Tipo de reporte no válido" });
                }

                // Saludo genérico: si es un solo destinatario usa su nombre,
                // si son varios usa un saludo neutro.
                var saludo = request.Destinatarios.Count == 1
                    ? $"Estimado/a {request.Destinatarios[0].Nombre},"
                    : "Estimados/as,";

                var cuerpoHtml = $@"
            <h2>Reporte del Sistema de Ventas</h2>
            <p>{saludo}</p>
            <p>Se adjunta el reporte de <strong>{request.TipoReporte}</strong>
               generado el {DateTime.Now:dd/MM/yyyy HH:mm}.</p>
            <br/>
            <p>Saludos,<br/>{request.RemitenteNombre ?? "Sistema de Ventas"}</p>";

                var destinatariosServicio = request.Destinatarios
                    .Select(d => new DestinatarioCorreo
                    {
                        Nombre = d.Nombre,
                        Correo = d.Correo
                    })
                    .ToList();

                await _correoService.EnviarReporteAsync(
                    destinatariosServicio,
                    $"Reporte {request.TipoReporte} - {DateTime.Now:dd/MM/yyyy}",
                    cuerpoHtml,
                    pdf,
                    nombreArchivo,
                    request.RemitenteNombre,
                    request.RemitenteEmail);

                return Ok(new { mensaje = "Correo enviado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }



        // ─── DESCARGAR PDF ────────────────────────────────────

        [HttpGet("ventas/pdf")]
        public async Task<IActionResult> DescargarPdfVentas(
    [FromQuery] string desde = null,
    [FromQuery] string hasta = null,
    [FromQuery] string estado = null,
    [FromQuery] string tipoPago = null)
        {
            try
            {
                var ventas = await _ventaService.GetNotasVentaAsync();

                if (!string.IsNullOrEmpty(desde))
                    ventas = ventas.FindAll(v => v.Fecha >= DateTime.Parse(desde));
                if (!string.IsNullOrEmpty(hasta))
                    ventas = ventas.FindAll(v => v.Fecha <= DateTime.Parse(hasta));
                if (!string.IsNullOrEmpty(estado) && estado != "todos")
                    ventas = ventas.FindAll(v => v.Estado == estado);
                if (!string.IsNullOrEmpty(tipoPago) && tipoPago != "todos")
                    ventas = ventas.FindAll(v => v.TipoPago == tipoPago);

                var totalVendido = 0m;
                var canceladas = 0;
                foreach (var v in ventas)
                {
                    if (v.Estado == "activo" || v.Estado == "pagado") totalVendido += v.MontoTotal;
                    if (v.Estado == "cancelado") canceladas++;
                }

                var pdf = _pdfService.GenerarReporteVentas(ventas, totalVendido, canceladas);
                return File(pdf, "application/pdf", $"reporte-ventas-{DateTime.Now:yyyyMMdd}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = ex.Message,
                    tipo = ex.GetType().Name,
                    stackTrace = ex.StackTrace,
                    inner = ex.InnerException?.Message
                });
            }
        }

        [HttpGet("compras/pdf")]
        public async Task<IActionResult> DescargarPdfCompras(
    [FromQuery] string desde = null,
    [FromQuery] string hasta = null,
    [FromQuery] string estado = null)
        {
            try
            {
                var compras = await _compraService.GetNotasCompraAsync();

                if (!string.IsNullOrEmpty(desde))
                    compras = compras.FindAll(c => c.FechaCompra >= DateTime.Parse(desde));
                if (!string.IsNullOrEmpty(hasta))
                    compras = compras.FindAll(c => c.FechaCompra <= DateTime.Parse(hasta));
                if (!string.IsNullOrEmpty(estado) && estado != "todos")
                    compras = compras.FindAll(c => c.Estado == estado);

                var totalComprado = 0m;
                foreach (var c in compras) totalComprado += c.TotalCompra;

                var pdf = _pdfService.GenerarReporteCompras(compras, totalComprado);
                return File(pdf, "application/pdf", $"reporte-compras-{DateTime.Now:yyyyMMdd}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpGet("inventario/pdf")]
        public async Task<IActionResult> DescargarPdfInventario()
        {
            try
            {
                var articulos = await _inventarioService.GetArticulosAsync();
                var pdf = _pdfService.GenerarReporteInventario(articulos);
                return File(pdf, "application/pdf", $"reporte-inventario-{DateTime.Now:yyyyMMdd}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }
        [HttpGet("movimientos/pdf")]
        public async Task<IActionResult> DescargarPdfMovimientos(
        [FromQuery] string desde = null,
        [FromQuery] string hasta = null)
        {
            try
            {
                var ingresos = await _inventarioService.GetIngresosAsync(incluirDetalle: true);
                var egresos = await _inventarioService.GetEgresosAsync(incluirDetalle: true);

                if (!string.IsNullOrEmpty(desde))
                {
                    var fechaDesde = DateTime.Parse(desde);
                    ingresos = ingresos.FindAll(i => i.Fecha >= fechaDesde);
                    egresos = egresos.FindAll(e => e.Fecha >= fechaDesde);
                }
                if (!string.IsNullOrEmpty(hasta))
                {
                    var fechaHasta = DateTime.Parse(hasta);
                    ingresos = ingresos.FindAll(i => i.Fecha <= fechaHasta);
                    egresos = egresos.FindAll(e => e.Fecha <= fechaHasta);
                }

                var pdf = _pdfService.GenerarReporteMovimientos(ingresos, egresos);
                return File(pdf, "application/pdf", $"reporte-movimientos-{DateTime.Now:yyyyMMdd}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }
    }


    public class EnviarCorreoRequest
    {
        public List<DestinatarioDto> Destinatarios { get; set; }
        public string TipoReporte { get; set; }
        public string RemitenteNombre { get; set; }
        public string RemitenteEmail { get; set; }
    }

    public class DestinatarioDto
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
    }
}