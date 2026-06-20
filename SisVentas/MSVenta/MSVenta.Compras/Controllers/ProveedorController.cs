using Microsoft.AspNetCore.Mvc;
using MSVenta.Compras.Models;
using MSVenta.Compras.Services;
using System.Threading.Tasks;

namespace MSVenta.Compras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        private readonly IProveedorService _service;

        public ProveedorController(IProveedorService service)
        {
            _service = service;
        }

        // GET api/proveedor
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var proveedores = await _service.GetAllAsync();
            return Ok(proveedores);
        }

        // GET api/proveedor/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var proveedor = await _service.GetByIdAsync(id);
            if (proveedor == null) return NotFound();
            return Ok(proveedor);
        }

        // POST api/proveedor
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Proveedor proveedor)
        {
            var created = await _service.AddAsync(proveedor);
            return Ok(created);
        }

        // PUT api/proveedor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Proveedor proveedor)
        {
            if (id != proveedor.Id) return BadRequest();
            var updated = await _service.UpdateAsync(proveedor);
            if (!updated) return NotFound();
            return NoContent();
        }

        // PUT api/proveedor/5/toggle
        [HttpPut("{id}/toggle")]
        public async Task<IActionResult> ToggleEstado(int id)
        {
            var result = await _service.ToggleEstadoAsync(id);
            if (!result) return NotFound();
            return Ok(new { mensaje = "Estado actualizado correctamente" });
        }
    }
}