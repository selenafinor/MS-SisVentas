namespace MSVenta.Reportes.DTOs
{
    public class ArticuloReporteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Estado { get; set; }
        public string NombreMarca { get; set; }
        public string NombreCategoria { get; set; }
        public int Stock { get; set; }
        public int StockMin { get; set; }
        public int StockMax { get; set; }
    }
}