using System.Collections.Generic;
namespace MSVenta.Seguridad.DTOs
{
    public class UsuarioDTO
    {
        public int UserId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Estado { get; set; }
        public List<RolDTO> Roles { get; set; }
    }
}