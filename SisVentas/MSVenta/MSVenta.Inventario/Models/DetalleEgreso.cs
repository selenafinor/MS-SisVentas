namespace MSVenta.Inventario.Models
{
    public class DetalleEgreso
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public string Observacion { get; set; }
        public int Id_Egreso { get; set; }
        public Egreso Egreso { get; set; }
        public int Id_ArticuloAlmacen { get; set; }
        public ArticuloAlmacen ArticuloAlmacen { get; set; }
    }
}