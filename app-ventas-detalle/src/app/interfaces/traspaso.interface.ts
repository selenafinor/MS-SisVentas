export interface Traspaso {
  id?: number;
  fecha?: string;
  glosa?: string;
  estado?: string;
  id_Usuario?: number;
  id_AlmacenOrigen?: number;
  id_AlmacenDestino?: number;
  almacenOrigen?: { id?: number; nombre?: string };
  almacenDestino?: { id?: number; nombre?: string };
}

export interface DetalleTraspaso {
  id?: number;
  cantidad?: number;
  id_Traspaso?: number;
  id_ArticuloAlmacen?: number;
  articuloAlmacen?: any;
}