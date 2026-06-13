namespace MSVenta.Inventario.DTOs
{
    public class ArticuloDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Estado { get; set; }
        public string Foto { get; set; }
        public int Id_Marca { get; set; }
        public string NombreMarca { get; set; }
        public int Id_Categoria { get; set; }
        public string NombreCategoria { get; set; }
        public int Id_UnidadMedida { get; set; }
        public string NombreUnidadMedida { get; set; }
        public int Stock { get; set; }
        public int StockMin { get; set; }
        public int StockMax { get; set; }
    }
}