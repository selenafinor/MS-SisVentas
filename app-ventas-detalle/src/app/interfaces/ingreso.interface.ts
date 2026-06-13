export interface Ingreso {
  id?: number;
  fecha?: string;
  glosa?: string;
  motivo?: string;
  estado?: string;
  id_Usuario?: number;
}

export interface DetalleIngreso {
  id?: number;
  cantidad?: number;
  precioCompra?: number;
  observacion?: string;
  id_Ingreso?: number;
  id_ArticuloAlmacen?: number;
}