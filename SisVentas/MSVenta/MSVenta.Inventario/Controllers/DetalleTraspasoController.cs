using Microsoft.AspNetCore.Mvc;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Services;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleTraspasoController : ControllerBase
    {
        private readonly IDetalleTraspasoService _service;

        public DetalleTraspasoController(IDetalleTraspasoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var detalles = await _service.GetAllAsync();
            return Ok(detalles);
        }
        [HttpGet("traspaso/{traspasoId}")]
        public async Task<IActionResult> GetByTraspasoId(int traspasoId)
        {
            var detalles = await _service.GetByTraspasoIdAsync(traspasoId);
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
        public async Task<IActionResult> Add([FromBody] DetalleTraspaso detalle)
        {
            var result = await _service.AddAsync(detalle);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DetalleTraspaso detalle)
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