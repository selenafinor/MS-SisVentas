using Microsoft.AspNetCore.Mvc;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Services;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngresoController : ControllerBase
    {
        private readonly IIngresoService _service;

        public IngresoController(IIngresoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ingresos = await _service.GetAllAsync();
            return Ok(ingresos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ingreso = await _service.GetByIdAsync(id);
            if (ingreso == null) return NotFound();
            return Ok(ingreso);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Ingreso ingreso)
        {
            var result = await _service.AddAsync(ingreso);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Ingreso ingreso)
        {
            ingreso.Id = id;
            var result = await _service.UpdateAsync(ingreso);
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