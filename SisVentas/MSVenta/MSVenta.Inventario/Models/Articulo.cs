namespace MSVenta.Inventario.Models
{
    public class Articulo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Estado { get; set; }
        public string Foto { get; set; }
        public int Id_Marca { get; set; }
        public Marca Marca { get; set; }
        public int Id_Categoria { get; set; }
        public Categoria Categoria { get; set; }
        public int Id_UnidadMedida { get; set; }
        public UnidadMedida UnidadMedida { get; set; }
    }
}