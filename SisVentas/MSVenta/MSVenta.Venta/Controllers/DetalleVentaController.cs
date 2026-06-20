using Microsoft.AspNetCore.Mvc;
using MSVenta.Venta.Models;
using MSVenta.Venta.Services;
using System;
using System.Threading.Tasks;

namespace MSVenta.Venta.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetalleVentaController : ControllerBase
    {
        private readonly IDetalleVentaService _service;

        public DetalleVentaController(IDetalleVentaService service)
        {
            _service = service;
        }

        // GET api/detalleventa
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var detalles = await _service.GetAllDetalles();
            return Ok(detalles);
        }

        // GET api/detalleventa/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var detalle = await _service.GetDetalle(id);
            if (detalle == null) return NotFound();
            return Ok(detalle);
        }

        // GET api/detalleventa/venta/5
        [HttpGet("venta/{ventaId}")]
        public async Task<IActionResult> GetByVenta(int ventaId)
        {
            var detalles = await _service.GetDetallesPorVenta(ventaId);
            return Ok(detalles);
        }

        // POST api/detalleventa
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DetalleVenta detalle)
        {
            try
            {
                await _service.CreateDetalle(detalle);
                return Ok(detalle);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // PUT api/detalleventa/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DetalleVenta detalle)
        {
            if (id != detalle.Id) return BadRequest();
            await _service.UpdateDetalle(detalle);
            return Ok(detalle);
        }

        // DELETE api/detalleventa/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteDetalle(id);
            return Ok();
        }
    }
}