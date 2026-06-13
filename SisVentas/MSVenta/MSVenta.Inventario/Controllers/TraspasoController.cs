using Microsoft.AspNetCore.Mvc;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Services;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraspasoController : ControllerBase
    {
        private readonly ITraspasoService _service;

        public TraspasoController(ITraspasoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var traspasos = await _service.GetAllAsync();
            return Ok(traspasos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var traspaso = await _service.GetByIdAsync(id);
            if (traspaso == null) return NotFound();
            return Ok(traspaso);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Traspaso traspaso)
        {
            var result = await _service.AddAsync(traspaso);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Traspaso traspaso)
        {
            traspaso.Id = id;
            var result = await _service.UpdateAsync(traspaso);
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