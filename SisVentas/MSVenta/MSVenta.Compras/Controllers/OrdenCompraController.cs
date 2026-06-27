using Microsoft.AspNetCore.Mvc;
using MSVenta.Compras.Models;
using MSVenta.Compras.Services;
using System;
using System.Threading.Tasks;
namespace MSVenta.Compras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenCompraController : ControllerBase
    {
        private readonly IOrdenCompraService _service;
        public OrdenCompraController(IOrdenCompraService service)
        {
            _service = service;
        }

        // GET api/ordencompra
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ordenes = await _service.GetAllAsync();
            return Ok(ordenes);
        }

        // GET api/ordencompra/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var orden = await _service.GetByIdAsync(id);
            if (orden == null) return NotFound();
            return Ok(orden);
        }

        // POST api/ordencompra
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrdenCompra orden)
        {
            var created = await _service.CreateAsync(orden);
            return Ok(created);
        }

        // POST api/ordencompra/5/detalle
        [HttpPost("{id}/detalle")]
        public async Task<IActionResult> AgregarDetalle(int id, [FromBody] DetalleOrdenCompra detalle)
        {
            try
            {
                var created = await _service.AgregarDetalleAsync(id, detalle);
                return Ok(created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // PUT api/ordencompra/detalle/5
        [HttpPut("detalle/{idDetalle}")]
        public async Task<IActionResult> ActualizarDetalle(int idDetalle, [FromBody] DetalleOrdenCompra detalle)
        {
            try
            {
                var updated = await _service.ActualizarDetalleAsync(idDetalle, detalle.Cantidad, detalle.PrecioUni);
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // DELETE api/ordencompra/detalle/5
        [HttpDelete("detalle/{idDetalle}")]
        public async Task<IActionResult> EliminarDetalle(int idDetalle)
        {
            var result = await _service.EliminarDetalleAsync(idDetalle);
            if (!result) return NotFound();
            return Ok(new { mensaje = "Artículo eliminado de la orden" });
        }

        // PUT api/ordencompra/5/estado
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> ActualizarEstado(int id, [FromBody] EstadoDto dto)
        {
            var result = await _service.ActualizarEstadoAsync(id, dto.Estado);
            if (!result) return NotFound();
            return Ok(new { mensaje = "Estado actualizado" });
        }
    }

    public class EstadoDto
    {
        public string Estado { get; set; }
    }
}