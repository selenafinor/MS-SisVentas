import { Proveedor } from './proveedor.interface';
import { OrdenCompra } from './orden-compra.interface';

export interface Adquisicion {
  id?: number;
  fecha?: string;
  estado?: string;
  glosa?: string;
  ordenId?: number;
  orden?: OrdenCompra;
  proveedorId?: number;
  proveedor?: Proveedor;
  usuarioId?: number;
  detalles?: DetalleAdquisicion[];
}

export interface DetalleAdquisicion {
  id?: number;
  adquisicionId?: number;
  productoId?: number;
  almacenId?: number;
  nombreProducto?: string;
  cantidad?: number;
  precioUni?: number;
  subTotal?: number;
}