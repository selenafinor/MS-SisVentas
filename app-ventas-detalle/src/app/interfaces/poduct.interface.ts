import { Category } from "./category.interface";

export interface Product {
  idProducto?: number;
  id?: number;
  descripcion?: string;
  nombre?: string;
  precio: number;
  stock?: number;
  estado?: string;
  idCategoria?: number;
  id_Categoria?: number;
  id_categoria?: number;
  categoria?: Category;
  productoId?: number;
  nombreProducto?: string;
  id_Marca?: number;
  id_UnidadMedida?: number;
  marca?: { id: number; nombre: string };
  unidadMedida?: { id: number; nombre: string; abreviatura: string };
  // nuevos campos del DTO
  nombreMarca?: string;
  nombreCategoria?: string;
  nombreUnidadMedida?: string;
  stockMin?: number;
  stockMax?: number;
  foto?: string;
}