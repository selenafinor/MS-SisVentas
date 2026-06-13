using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSVenta.Seguridad.Models
{
    public class Rol
    {
        [Key]
        [Column("ID_Rol")]
        public int ID_Rol { get; set; }
        public string Nombre_Rol { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; } = "activo";
        public ICollection<RolPermiso> RolPermisos { get; set; }
    }
}