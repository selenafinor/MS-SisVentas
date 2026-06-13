
using System;
namespace MSVenta.Inventario.Models
{
    public class Traspaso
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Glosa { get; set; }
        public string Estado { get; set; }
        public int Id_Usuario { get; set; }
        public int Id_AlmacenOrigen { get; set; }
        public Almacen AlmacenOrigen { get; set; }
        public int Id_AlmacenDestino { get; set; }
        public Almacen AlmacenDestino { get; set; }
    }
}