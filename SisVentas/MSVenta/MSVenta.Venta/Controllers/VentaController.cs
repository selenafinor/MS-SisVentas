using Microsoft.AspNetCore.Mvc;
using MSVenta.Venta.Models;
using MSVenta.Venta.Services;
using System;
using System.Threading.Tasks;

namespace MSVenta.Venta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _service;
        private readonly LibelulaService _libelula;

        public VentaController(IVentaService service, LibelulaService libelula)
        {
            _service = service;
            _libelula = libelula;
        }

        // GET api/venta
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ventas = await _service.GetAllVentas();
            return Ok(ventas);
        }

        // GET api/venta/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var venta = await _service.GetVenta(id);
            if (venta == null) return NotFound();
            return Ok(venta);
        }

        // POST api/venta
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Models.Venta venta)
        {
            try
            {
                await _service.CreateVenta(venta);
                return Ok(venta);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // POST api/venta/5/detalle
        [HttpPost("{id}/detalle")]
        public async Task<IActionResult> AgregarDetalle(int id, [FromBody] DetalleVenta detalle)
        {
            try
            {
                await _service.AgregarDetalle(id, detalle);
                return Ok(new { mensaje = "Artículo agregado correctamente" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
        // POST api/venta/5/confirmar-stock
        [HttpPost("{id}/confirmar-stock")]
        public async Task<IActionResult> ConfirmarStock(int id)
        {
            try
            {
                await _service.ConfirmarStock(id);
                return Ok(new { mensaje = "Stock confirmado, procesando en inventario..." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
        // PUT api/venta/5/cancelar
        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(int id)
        {
            try
            {
                await _service.CancelarVenta(id);
                return Ok(new { mensaje = "Venta cancelada correctamente" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // PUT api/venta/5/confirmar
        [HttpPut("{id}/confirmar")]
        public async Task<IActionResult> ConfirmarPago(int id)
        {
            try
            {
                await _service.ConfirmarPago(id);
                return Ok(new { mensaje = "Pago confirmado correctamente" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // POST api/venta/5/generarQr
        [HttpPost("{id}/generarQr")]
        public async Task<IActionResult> GenerarQr(int id)
        {
            var venta = await _service.GetVenta(id);
            if (venta == null)
                return NotFound(new { ok = false, mensaje = "Venta no encontrada" });

            if (venta.Estado == "cancelado")
                return BadRequest(new { ok = false, mensaje = "No se puede generar QR para una venta cancelada" });

            if (venta.MontoTotal <= 0)
                return BadRequest(new { ok = false, mensaje = "La venta no tiene artículos. Agrega artículos antes de generar el QR." });

            var resultado = await _libelula.RegistrarDeudaVenta(venta);

            if (resultado.Ok)
            {
                await _service.AsignarTransaccionQr(id, resultado.IdTransaccion);
                return Ok(new
                {
                    ok = true,
                    mensaje = resultado.Mensaje,
                    idTransaccion = resultado.IdTransaccion,
                    urlPasarela = resultado.UrlPasarela,
                    qrUrl = resultado.QrUrl
                });
            }

            return BadRequest(new { ok = false, mensaje = resultado.Mensaje });
        }

        // GET api/venta/5/estadoPago
        [HttpGet("{id}/estadoPago")]
        public async Task<IActionResult> EstadoPago(int id)
        {
            var venta = await _service.GetVenta(id);
            if (venta == null)
                return NotFound(new { error = "Venta no encontrada" });

            if (venta.Estado == "pagado")
                return Ok(new { estado = venta.Estado, pagoConfirmado = venta.PagoConfirmado, montoTotal = venta.MontoTotal });

            if (venta.TipoPago == "qr" && venta.Estado == "activo" && !string.IsNullOrEmpty(venta.IdTransaccionQr))
            {
                var consulta = await _libelula.ConsultarPago(venta.IdTransaccionQr);
                if (consulta.Ok && consulta.Pagado)
                    await _service.ConfirmarPago(id);
            }

            venta = await _service.GetVenta(id);
            return Ok(new { estado = venta.Estado, pagoConfirmado = venta.PagoConfirmado, montoTotal = venta.MontoTotal });
        }

        // DELETE api/venta/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteVenta(id);
            return Ok();
        }
    }
}