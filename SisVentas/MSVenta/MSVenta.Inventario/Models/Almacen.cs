namespace MSVenta.Inventario.Models
{
    public class Almacen
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public int CantidadMax { get; set; }
        public string Estado { get; set; }
    }
}