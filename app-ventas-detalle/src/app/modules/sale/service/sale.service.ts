import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../environments/environment.development';
import { Sale } from '../../../interfaces/sale.interface';
import { SaleDetail } from '../../../interfaces/sale-detail.inteface';

function httpOptions(token: string) {
  return {
    headers: new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    })
  };
}

@Injectable({
  providedIn: 'root'
})
export class SaleService {
  private apiUrl = `${environment.URL_SERVICIOS}/venta`;

  constructor(private http: HttpClient) { }

  // Obtener todas las ventas
  getSalesAll(): Observable<Sale[]> {
    const token = sessionStorage.getItem('token');
    if (!token) return of([]);
    return this.http.get<Sale[]>(this.apiUrl, httpOptions(token))
      .pipe(catchError(this.handleError<Sale[]>('getSalesAll', [])));
  }

  // Obtener venta por id
  getSaleById(id: number): Observable<Sale> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.get<Sale>(`${this.apiUrl}/${id}`, httpOptions(token))
      .pipe(catchError(this.handleError<Sale>('getSaleById', {})));
  }

  // Crear venta
  createSale(sale: Sale): Observable<Sale> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.post<Sale>(this.apiUrl, sale, httpOptions(token))
      .pipe(catchError(this.handleError<Sale>('createSale', {})));
  }

  // Agregar detalle a una venta
  agregarDetalle(idVenta: number, detalle: SaleDetail): Observable<any> {
  const token = sessionStorage.getItem('token');
  if (!token) return of({});

  // Mapear campos del frontend al formato que espera el backend
  const body = {
    id_Producto:     detalle.idProducto,
    nombreProducto:  detalle.nombreProducto,
    id_Almacen:      detalle.idAlmacen,
    nombreAlmacen:   detalle.nombreAlmacen,
    cantidad:        detalle.cantidad,
    precioUni:       detalle.precioUni,
    precioSubtotal:  detalle.precioSubtotal,
    ventaId:         0,
    id:              0
  };

  return this.http.post<any>(`${this.apiUrl}/${idVenta}/detalle`, body, httpOptions(token))
    .pipe(catchError(this.handleError<any>('agregarDetalle', {})));
}

  // Cancelar venta
  cancelarVenta(id: number): Observable<any> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.put<any>(`${this.apiUrl}/${id}/cancelar`, {}, httpOptions(token))
      .pipe(catchError(this.handleError<any>('cancelarVenta', {})));
  }

  // Confirmar pago
  confirmarPago(id: number): Observable<any> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.put<any>(`${this.apiUrl}/${id}/confirmar`, {}, httpOptions(token))
      .pipe(catchError(this.handleError<any>('confirmarPago', {})));
  }

  // Eliminar venta
  deleteSale(id: number): Observable<any> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.delete<any>(`${this.apiUrl}/${id}`, httpOptions(token))
      .pipe(catchError(this.handleError<any>('deleteSale', {})));
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed:`, error);
      return of(result as T);
    };
  }
}