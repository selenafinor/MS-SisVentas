using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MSVenta.Seguridad.Models
{
    public class Usuario
    {
        [Key]
        public int UserId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Estado { get; set; } = "activo";
        public ICollection<RolPermisoUsuario> RolPermisoUsuarios { get; set; }
    }
}