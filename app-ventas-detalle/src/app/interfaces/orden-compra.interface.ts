import { Proveedor } from './proveedor.interface';
import { DetalleOrdenCompra } from './detalle-orden-compra.interface';

export interface OrdenCompra {
  id?: number;
  fecha?: string;
  estado?: string;
  glosa?: string;
  total?: number;
  proveedorId?: number;
  proveedor?: Proveedor;
  usuarioId?: number;
  detalles?: DetalleOrdenCompra[];
}