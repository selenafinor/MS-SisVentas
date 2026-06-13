export interface Almacen {
  id?: number;
  nombre?: string;
  descripcion?: string;
  direccion?: string;
  cantidadMax?: number;
  estado?: string;
  stockTotal?: number;
  articulosAlmacen?: any[];
}