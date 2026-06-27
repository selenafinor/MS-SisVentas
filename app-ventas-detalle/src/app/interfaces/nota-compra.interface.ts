import { Proveedor } from './proveedor.interface';

export interface NotaCompra {
  id?: number;
  fechaCompra?: string;
  totalCompra?: number;
  estado?: string;
  glosa?: string;
  tipoPago?: string;
  proveedorId?: number;
  proveedor?: Proveedor;
  usuarioId?: number;
  detalles?: DetalleNotaCompra[];
}

export interface DetalleNotaCompra {
  id?: number;
  compraId?: number;
  productoId?: number;
  nombreProducto?: string;
  cantidad?: number;
  precioUni?: number;
  subTotal?: number;
}