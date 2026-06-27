using Microsoft.AspNetCore.Mvc;
using MSVenta.Compras.Models;
using MSVenta.Compras.Services;
using System;
using System.Threading.Tasks;
namespace MSVenta.Compras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdquisicionController : ControllerBase
    {
        private readonly IAdquisicionService _service;
        public AdquisicionController(IAdquisicionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var adq = await _service.GetByIdAsync(id);
            if (adq == null) return NotFound();
            return Ok(adq);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Adquisicion adquisicion)
        {
            return Ok(await _service.CreateAsync(adquisicion));
        }

        [HttpPost("{id}/detalle")]
        public async Task<IActionResult> AgregarDetalle(int id, [FromBody] DetalleAdquisicion detalle)
        {
            try
            {
                return Ok(await _service.AgregarDetalleAsync(id, detalle));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

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
    }
}