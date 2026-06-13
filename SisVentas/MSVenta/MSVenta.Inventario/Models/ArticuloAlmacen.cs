namespace MSVenta.Inventario.Models
{
    public class ArticuloAlmacen
    {
        public int Id { get; set; }
        public int Stock { get; set; }
        public int StockMin { get; set; }
        public int StockMax { get; set; }
        public int Id_Articulo { get; set; }
        public Articulo Articulo { get; set; }
        public int Id_Almacen { get; set; }
        public Almacen Almacen { get; set; }
    }
}