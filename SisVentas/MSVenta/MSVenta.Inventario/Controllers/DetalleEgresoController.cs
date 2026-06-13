using Microsoft.AspNetCore.Mvc;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Services;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleEgresoController : ControllerBase
    {
        private readonly IDetalleEgresoService _service;

        public DetalleEgresoController(IDetalleEgresoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var detalles = await _service.GetAllAsync();
            return Ok(detalles);
        }
        [HttpGet("egreso/{egresoId}")]
        public async Task<IActionResult> GetByEgresoId(int egresoId)
        {
            var detalles = await _service.GetByEgresoIdAsync(egresoId);
            return Ok(detalles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var detalle = await _service.GetByIdAsync(id);
            if (detalle == null) return NotFound();
            return Ok(detalle);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] DetalleEgreso detalle)
        {
            var result = await _service.AddAsync(detalle);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DetalleEgreso detalle)
        {
            detalle.Id = id;
            var result = await _service.UpdateAsync(detalle);
            if (!result) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return Ok(result);
        }
    }
}