import { Customer } from "./customer.interface";
import { SaleDetail } from "./sale-detail.inteface";

export interface Sale {
  id?: number;
  fecha?: string;
  hora?: string;
  montoTotal?: number;
  glosa?: string;
  estado?: string;
  tipoPago?: string;
  pagoConfirmado?: boolean;
  idTransaccionQr?: string;
  clienteId?: number;
  cliente?: Customer;
  usuarioId?: number;
  detalles?: SaleDetail[];
}