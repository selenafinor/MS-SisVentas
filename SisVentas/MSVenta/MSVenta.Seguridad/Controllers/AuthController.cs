using Aforo255.Cross.Token.Src;
using Consul;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MSVenta.Seguridad.DTOs;
using MSVenta.Seguridad.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MSVenta.Seguridad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUsuarioService _accessService;
        private readonly JwtOptions _jwtOption;
        public AuthController(IUsuarioService accessService,
            IOptionsSnapshot<JwtOptions> jwtOption)
        {
            _accessService = accessService;
            _jwtOption = jwtOption.Value;
        }

        public IActionResult Get()
        {
            return Ok(_accessService.GetAllUsuarios());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AuthRequest request)
        {
            // Validamos las credenciales del usuario
            var resultado = await _accessService.Validate(request.UserName, request.Password);

            if (resultado.Motivo != MotivoLogin.Exitoso)
            {
                string mensaje;
                switch (resultado.Motivo)
                {
                    case MotivoLogin.UsuarioNoExiste:
                    case MotivoLogin.PasswordIncorrecta:
                        mensaje = "Usuario o contraseña incorrectos.";
                        break;
                    case MotivoLogin.CuentaInactiva:
                        mensaje = "Tu cuenta está deshabilitada. Contacta a un administrador.";
                        break;
                    case MotivoLogin.CuentaBloqueada:
                        var minutosRestantes = resultado.BloqueadoHasta.HasValue
                            ? Math.Ceiling((resultado.BloqueadoHasta.Value - DateTime.Now).TotalMinutes)
                            : 5;
                        mensaje = $"Cuenta bloqueada temporalmente por múltiples intentos fallidos. Intenta de nuevo en {minutosRestantes} minuto(s).";
                        break;
                    default:
                        mensaje = "No se pudo iniciar sesión.";
                        break;
                }

                return Unauthorized(new { message = mensaje });
            }

            var usuario = resultado.Usuario;

            // Obtenemos los permisos del usuario
            UsuarioDTO userPermisos = await _accessService.GetUsuarioById(usuario.UserId);
            // Generamos el token JWT
            var token = JwtToken.Create(_jwtOption);
            // Configuramos los headers de respuesta
            Response.Headers.Add("access-control-expose-headers", "Authorization");
            Response.Headers.Add("Authorization", token);
            // Estructuramos la respuesta de manera clara
            var response = new
            {
                user = new
                {
                    userPermisos.UserId,
                    userPermisos.Fullname,
                    userPermisos.Username,
                    userPermisos.Correo,
                    Roles = userPermisos.Roles.Select(role => new
                    {
                        role.ID_Rol,
                        role.Nombre_Rol,
                        Permisos = role.Permisos.Select(permiso => new
                        {
                            permiso.ID_Permiso,
                            permiso.Nombre_Permiso
                        })
                    }),
                    token,
                }
            };
            return Ok(response);
        }
    }
}
