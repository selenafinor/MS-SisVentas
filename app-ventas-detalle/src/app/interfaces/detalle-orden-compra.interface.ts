export interface DetalleOrdenCompra {
  id?: number;
  ordenId?: number;
  productoId?: number;
  nombreProducto?: string;
  cantidad?: number;
  precioUni?: number;
  subTotal?: number;
}