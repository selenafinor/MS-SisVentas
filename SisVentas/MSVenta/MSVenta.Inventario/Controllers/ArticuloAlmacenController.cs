using Microsoft.AspNetCore.Mvc;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Services;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticuloAlmacenController : ControllerBase
    {
        private readonly IArticuloAlmacenService _service;

        public ArticuloAlmacenController(IArticuloAlmacenService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var articulos = await _service.GetAllAsync();
            return Ok(articulos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var articulo = await _service.GetByIdAsync(id);
            if (articulo == null) return NotFound();
            return Ok(articulo);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ArticuloAlmacen articuloAlmacen)
        {
            var result = await _service.AddAsync(articuloAlmacen);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ArticuloAlmacen articuloAlmacen)
        {
            articuloAlmacen.Id = id;
            var result = await _service.UpdateAsync(articuloAlmacen);
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