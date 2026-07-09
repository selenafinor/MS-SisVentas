using Microsoft.AspNetCore.Mvc;
using MSVenta.Seguridad.Models;
using MSVenta.Seguridad.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Seguridad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolPermisoUsuariosController : Controller
    {

        private readonly IRolPermisoUsuarioService _rolPermisoUsuarioService;

        public RolPermisoUsuariosController(IRolPermisoUsuarioService rolPermisoUsuarioService)
        {
            _rolPermisoUsuarioService = rolPermisoUsuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolPermisoUsuario>>> GetRolPermisoUsuarios()
        {
            var rolpermisousuarioas = await _rolPermisoUsuarioService.GetAllRolPermisoUsuarios();
            return Ok(rolpermisousuarioas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RolPermisoUsuario>> GetRolPermisoUsuario(int id)
        {
            var rolpermisosusuario = await _rolPermisoUsuarioService.GetRolPermisoUsuarioById(id);
            return rolpermisosusuario == null ? NotFound() : Ok(rolpermisosusuario);
        }

        [HttpPost]
        public async Task<ActionResult<RolPermisoUsuario>> CreateRolPermisoUsuario(RolPermisoUsuario rolpermisousuario)
        {
            var createdRolPermisoUsuario = await _rolPermisoUsuarioService.CreateRolPermisoUsuario(rolpermisousuario);
            return CreatedAtAction(nameof(GetRolPermisoUsuarios), new { id = createdRolPermisoUsuario.ID_Usuario_Rol_Permiso }, createdRolPermisoUsuario);
        }

        // ANTES este endpoint no existia: el frontend nunca podia borrar una
        // asignacion rol-usuario anterior, por eso al "cambiar" el rol de un
        // usuario en realidad se iba acumulando uno nuevo encima del viejo.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRolPermisoUsuario(int id)
        {
            var existente = await _rolPermisoUsuarioService.GetRolPermisoUsuarioById(id);
            if (existente == null)
                return NotFound();

            await _rolPermisoUsuarioService.DeleteRolPermisoUsuario(id);
            return NoContent();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}