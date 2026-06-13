using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Services;
using System.IO;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticuloController : ControllerBase
    {
        private readonly IArticuloService _service;
        private readonly IWebHostEnvironment _env;

        public ArticuloController(IArticuloService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
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
        public async Task<IActionResult> Add([FromBody] Articulo articulo)
        {
            var result = await _service.AddAsync(articulo);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Articulo articulo)
        {
            articulo.Id = id;
            var result = await _service.UpdateAsync(articulo);
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

        [HttpPost("upload/{id}")]
        public async Task<IActionResult> UploadFoto(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No se envió ningún archivo.");

            var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"articulo_{id}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fotoUrl = $"/uploads/{fileName}";
            await _service.UpdateFotoAsync(id, fotoUrl);

            return Ok(new { url = fotoUrl });
        }
    }
}