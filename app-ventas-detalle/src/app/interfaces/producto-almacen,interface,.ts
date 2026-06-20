import { Almacen } from "./almacen.interface";
import { Product } from "./poduct.interface";

export interface ProductoAlmacen {
  id?: number;
  stock?: number;
  stockMin?: number;
  stockMax?: number;
  id_Articulo?: number;
  id_Almacen?: number;
  // campos anteriores para compatibilidad
  productoId?: number;
  almacenId?: number;
  producto?: Product;
  articulo?: Product;
  almacen?: Almacen;
}