using Microsoft.AspNetCore.Mvc;
using MSVenta.Compras.Models;
using MSVenta.Compras.Services;
using System.Threading.Tasks;

namespace MSVenta.Compras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogoProveedorController : ControllerBase
    {
        private readonly ICatalogoProveedorService _service;

        public CatalogoProveedorController(ICatalogoProveedorService service)
        {
            _service = service;
        }

        // GET api/catalogoproveedor
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        // GET api/catalogoproveedor/proveedor/5
        [HttpGet("proveedor/{proveedorId}")]
        public async Task<IActionResult> GetByProveedor(int proveedorId)
        {
            var items = await _service.GetByProveedorIdAsync(proveedorId);
            return Ok(items);
        }

        // GET api/catalogoproveedor/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST api/catalogoproveedor
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CatalogoProveedor item)
        {
            var created = await _service.AddAsync(item);
            return Ok(created);
        }

        // PUT api/catalogoproveedor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CatalogoProveedor item)
        {
            if (id != item.Id) return BadRequest();
            var updated = await _service.UpdateAsync(item);
            if (!updated) return NotFound();
            return NoContent();
        }

        // DELETE api/catalogoproveedor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return Ok(new { mensaje = "Eliminado correctamente" });
        }
        // GET api/catalogoproveedor/producto/5
        [HttpGet("producto/{productoId}")]
        public async Task<IActionResult> GetByProducto(int productoId)
        {
            var items = await _service.GetByProductoIdAsync(productoId);
            return Ok(items);
        }
    }
}