import { Proveedor } from './proveedor.interface';

export interface CatalogoProveedor {
  id?: number;
  precioUnitario?: number;
  stockDisponible?: number;
  estado?: string;
  nombreProducto?: string;
  proveedorId?: number;
  proveedor?: Proveedor;
  productoId?: number;
}