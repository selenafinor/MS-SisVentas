using Microsoft.AspNetCore.Mvc;
using MSVenta.Venta.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MSVenta.Venta.Controllers
{
    [ApiController]
    [Route("api/pago")]
    public class CallbackController : ControllerBase
    {
        private readonly IVentaService _service;

        public CallbackController(IVentaService service)
        {
            _service = service;
        }

        // GET /api/pago/callback?transaction_id=VENTA-10-20260618-143532&error=0
        [HttpGet("callback")]
        public async Task<IActionResult> CallbackGet(
            [FromQuery] string transaction_id,
            [FromQuery] string error = "1")
        {
            try
            {
                var partes = transaction_id?.Split('-');
                if (partes == null || partes.Length < 2)
                    return BadRequest(new { ok = false, mensaje = "ID venta no encontrado" });

                var idVenta = int.Parse(partes[1]);

                if (error == "0")
                    await _service.ConfirmarPago(idVenta);

                return Ok(new { ok = true, mensaje = "Estado actualizado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ok = false, mensaje = ex.Message });
            }
        }

        // POST /api/pago/callback — Libélula envía JSON
        [HttpPost("callback")]
        public async Task<IActionResult> CallbackPost([FromBody] JsonElement body)
        {
            try
            {
                var identificador = body.TryGetProperty("identificador", out var id)
                    ? id.GetString() : "";

                var estado = body.TryGetProperty("estado_transaccion", out var est)
                    ? est.GetRawText() : "";

                var partes = identificador?.Split('-');
                if (partes == null || partes.Length < 2)
                    return BadRequest(new { ok = false, mensaje = "ID venta no encontrado" });

                var idVenta = int.Parse(partes[1]);

                if (estado == "\"PAGADO\"" || estado == "\"COMPLETADO\"" ||
                    estado == "2" || estado == "\"2\"")
                    await _service.ConfirmarPago(idVenta);

                return Ok(new { ok = true, mensaje = "Estado actualizado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ok = false, mensaje = ex.Message });
            }
        }
    }
}