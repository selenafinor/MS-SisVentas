using Microsoft.AspNetCore.Mvc;
using MSVenta.Compras.Models;
using MSVenta.Compras.Services;
using System;
using System.Threading.Tasks;

namespace MSVenta.Compras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotaCompraController : ControllerBase
    {
        private readonly INotaCompraService _service;

        public NotaCompraController(INotaCompraService service)
        {
            _service = service;
        }

        // GET api/notacompra
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var compras = await _service.GetAllAsync();
            return Ok(compras);
        }

        // GET api/notacompra/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var compra = await _service.GetByIdAsync(id);
            if (compra == null) return NotFound();
            return Ok(compra);
        }

        // POST api/notacompra
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NotaCompra compra)
        {
            var created = await _service.CreateAsync(compra);
            return Ok(created);
        }

        // POST api/notacompra/5/detalle
        [HttpPost("{id}/detalle")]
        public async Task<IActionResult> AgregarDetalle(int id, [FromBody] DetalleCompra detalle)
        {
            try
            {
                var created = await _service.AgregarDetalleAsync(id, detalle);
                return Ok(created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // POST api/notacompra/5/confirmar-stock
        [HttpPost("{id}/confirmar-stock")]
        public async Task<IActionResult> ConfirmarStock(int id)
        {
            try
            {
                await _service.ConfirmarStockAsync(id);
                return Ok(new { mensaje = "Stock confirmado, procesando en inventario..." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // PUT api/notacompra/5/cancelar
        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(int id)
        {
            var result = await _service.CancelarAsync(id);
            if (!result) return BadRequest(new { mensaje = "No se pudo cancelar la compra" });
            return Ok(new { mensaje = "Compra cancelada correctamente" });
        }
    }
}