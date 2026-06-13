using Microsoft.AspNetCore.Mvc;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Services;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EgresoController : ControllerBase
    {
        private readonly IEgresoService _service;

        public EgresoController(IEgresoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var egresos = await _service.GetAllAsync();
            return Ok(egresos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var egreso = await _service.GetByIdAsync(id);
            if (egreso == null) return NotFound();
            return Ok(egreso);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Egreso egreso)
        {
            var result = await _service.AddAsync(egreso);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Egreso egreso)
        {
            egreso.Id = id;
            var result = await _service.UpdateAsync(egreso);
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