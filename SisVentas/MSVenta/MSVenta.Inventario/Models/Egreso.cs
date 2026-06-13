
using System;
namespace MSVenta.Inventario.Models
{
    public class Egreso
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Glosa { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }
        public int Id_Usuario { get; set; }
    }
}