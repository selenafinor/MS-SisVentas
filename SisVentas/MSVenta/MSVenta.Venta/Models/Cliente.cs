namespace MSVenta.Venta.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Nit { get; set; }
        public string Direccion { get; set; }
        public string Estado { get; set; }
    }
}