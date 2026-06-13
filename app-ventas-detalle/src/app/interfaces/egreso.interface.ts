export interface Egreso {
  id?: number;
  fecha?: string;
  glosa?: string;
  motivo?: string;
  estado?: string;
  id_Usuario?: number;
}

export interface DetalleEgreso {
  id?: number;
  cantidad?: number;
  observacion?: string;
  id_Egreso?: number;
  id_ArticuloAlmacen?: number;
}