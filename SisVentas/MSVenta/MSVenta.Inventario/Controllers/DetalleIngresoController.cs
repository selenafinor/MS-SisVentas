using Microsoft.AspNetCore.Mvc;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Services;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleIngresoController : ControllerBase
    {
        private readonly IDetalleIngresoService _service;

        public DetalleIngresoController(IDetalleIngresoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var detalles = await _service.GetAllAsync();
            return Ok(detalles);
        }

        [HttpGet("ingreso/{ingresoId}")]
        public async Task<IActionResult> GetByIngresoId(int ingresoId)
        {
            var detalles = await _service.GetByIngresoIdAsync(ingresoId);
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
        public async Task<IActionResult> Add([FromBody] DetalleIngreso detalle)
        {
            var result = await _service.AddAsync(detalle);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DetalleIngreso detalle)
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