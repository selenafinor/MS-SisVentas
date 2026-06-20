using Microsoft.AspNetCore.Mvc;
using MSVenta.Venta.Models;
using MSVenta.Venta.Services;
using System.Threading.Tasks;

namespace MSVenta.Venta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _service;

        public ClienteController(IClienteService service)
        {
            _service = service;
        }

        // GET api/cliente
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _service.GetAllClientes();
            return Ok(clientes);
        }

        // GET api/cliente/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _service.GetCliente(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        // GET api/cliente/buscar/carlos
        [HttpGet("buscar/{termino}")]
        public async Task<IActionResult> Buscar(string termino)
        {
            var clientes = await _service.BuscarClientes(termino);
            return Ok(clientes);
        }

        // POST api/cliente
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cliente cliente)
        {
            await _service.CreateCliente(cliente);
            return Ok(cliente);
        }

        // PUT api/cliente/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cliente cliente)
        {
            cliente.Id = id;
            await _service.UpdateCliente(cliente);
            return Ok(cliente);
        }

        // PUT api/cliente/5/toggle
        [HttpPut("{id}/toggle")]
        public async Task<IActionResult> Toggle(int id)
        {
            await _service.ToggleEstado(id);
            return Ok(new { mensaje = "Estado actualizado correctamente" });
        }

        // DELETE api/cliente/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteCliente(id);
            return Ok();
        }
    }
}