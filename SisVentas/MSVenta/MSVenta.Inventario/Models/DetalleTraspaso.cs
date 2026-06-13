namespace MSVenta.Inventario.Models
{
    public class DetalleTraspaso
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public int Id_Traspaso { get; set; }
        public Traspaso Traspaso { get; set; }
        public int Id_ArticuloAlmacen { get; set; }
        public ArticuloAlmacen ArticuloAlmacen { get; set; }
    }
}