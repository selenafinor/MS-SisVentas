namespace MSVenta.Inventario.Models
{
    public class DetalleIngreso
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioCompra { get; set; }
        public string Observacion { get; set; }
        public int Id_Ingreso { get; set; }
        public Ingreso Ingreso { get; set; }
        public int Id_ArticuloAlmacen { get; set; }
        public ArticuloAlmacen ArticuloAlmacen { get; set; }
    }
}