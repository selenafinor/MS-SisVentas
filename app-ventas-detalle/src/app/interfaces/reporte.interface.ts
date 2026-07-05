export interface Cliente {
  id?: number;
  nombre?: string;
  paterno?: string;
  materno?: string;
}

export interface DetalleVenta {
  id?: number;
  cantidad?: number;
  precioUni?: number;
  precioSubtotal?: number;
  nombreProducto?: string;
}

export interface NotaVenta {
  id?: number;
  fecha?: string;
  hora?: string;
  montoTotal?: number;
  glosa?: string;
  estado?: string;
  tipoPago?: string;
  clienteId?: number;
  cliente?: Cliente;
  detalles?: DetalleVenta[];
}

export interface ReporteVentasResponse {
  totalVendido: number;
  ventasCanceladas: number;
  totalRegistros: number;
  ventas: NotaVenta[];
}

export interface Proveedor {
  id?: number;
  nombre?: string;
}

export interface NotaCompra {
  id?: number;
  fechaCompra?: string;
  totalCompra?: number;
  estado?: string;
  glosa?: string;
  tipoPago?: string;
  proveedorId?: number;
  proveedor?: Proveedor;
}

export interface ReporteComprasResponse {
  totalComprado: number;
  totalRegistros: number;
  compras: NotaCompra[];
}

export interface ArticuloReporte {
  id?: number;
  nombre?: string;
  precio?: number;
  estado?: string;
  nombreMarca?: string;
  nombreCategoria?: string;
  stock?: number;
  stockMin?: number;
  stockMax?: number;
}

export interface ReporteInventarioResponse {
  totalArticulos: number;
  totalStockBajo: number;
  articulos: ArticuloReporte[];
}

export interface ArticuloDetalle {
  id?: number;
  nombre?: string;
}

export interface ArticuloAlmacenDetalle {
  id?: number;
  articulo?: ArticuloDetalle;
}

export interface DetalleMovimiento {
  id?: number;
  cantidad?: number;
  precioCompra?: number;
  observacion?: string;
  articuloAlmacen?: ArticuloAlmacenDetalle;
}

export interface Ingreso {
  id?: number;
  fecha?: string;
  glosa?: string;
  motivo?: string;
  estado?: string;
  soloRegistro?: boolean;
  detalles?: DetalleMovimiento[];
}

export interface Egreso {
  id?: number;
  fecha?: string;
  glosa?: string;
  motivo?: string;
  estado?: string;
  detalles?: DetalleMovimiento[];
}

export interface ReporteMovimientosResponse {
  totalIngresos: number;
  totalEgresos: number;
  ingresos: Ingreso[];
  egresos: Egreso[];
}

export interface TopArticulo {
  nombre: string;
  cantidad: number;
  total: number;
}

// ─── NUEVO: soporte para múltiples destinatarios ───────

export interface DestinatarioCorreo {
  nombre: string;
  correo: string;
}

export interface EnviarCorreoRequest {
  destinatarios: DestinatarioCorreo[];
  tipoReporte: string;
  remitenteNombre?: string;
  remitenteEmail?: string;
}
export interface OrdenCompraDashboard {
  id: number;
  fecha: string;
  total: number;
  estado: string;
  glosa: string;
  proveedorId?: number;
  proveedor?: { id: number; nombre: string };
}

export interface DashboardResponse {
  ventasHoyMonto: number;
  ventasHoyCantidad: number;
  totalArticulos: number;
  totalUnidadesEnStock: number;
  totalStockBajo: number;
  alertasStockBajo: ArticuloReporte[];
  ordenesPendientes: number;
  ultimasVentas: NotaVenta[];
}