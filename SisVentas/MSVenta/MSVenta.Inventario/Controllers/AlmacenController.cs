using Microsoft.AspNetCore.Mvc;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Services;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlmacenController : ControllerBase
    {
        private readonly IAlmacenService _service;

        public AlmacenController(IAlmacenService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var almacenes = await _service.GetAllAsync();
            return Ok(almacenes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var almacen = await _service.GetByIdAsync(id);
            if (almacen == null) return NotFound();
            return Ok(almacen);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Almacen almacen)
        {
            var result = await _service.AddAsync(almacen);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Almacen almacen)
        {
            almacen.Id = id;
            var result = await _service.UpdateAsync(almacen);
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