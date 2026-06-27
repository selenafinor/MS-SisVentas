import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../environments/environment.development';
import { OrdenCompra } from '../../../interfaces/orden-compra.interface';
import { DetalleOrdenCompra } from '../../../interfaces/detalle-orden-compra.interface';

function httpOptions(token: string) {
  return {
    headers: new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    })
  };
}

@Injectable({ providedIn: 'root' })
export class OrdenCompraService {
  private apiUrl = `${environment.URL_SERVICIOS}/ordencompra`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<OrdenCompra[]> {
    const token = sessionStorage.getItem('token');
    if (!token) return of([]);
    return this.http.get<OrdenCompra[]>(this.apiUrl, httpOptions(token))
      .pipe(catchError(this.handleError<OrdenCompra[]>('getAll', [])));
  }

  getById(id: number): Observable<OrdenCompra> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.get<OrdenCompra>(`${this.apiUrl}/${id}`, httpOptions(token))
      .pipe(catchError(this.handleError<OrdenCompra>('getById', {})));
  }

  create(orden: OrdenCompra): Observable<OrdenCompra> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.post<OrdenCompra>(this.apiUrl, orden, httpOptions(token))
      .pipe(catchError(this.handleError<OrdenCompra>('create', {})));
  }

  agregarDetalle(idOrden: number, detalle: DetalleOrdenCompra): Observable<any> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.post<any>(`${this.apiUrl}/${idOrden}/detalle`, detalle, httpOptions(token))
      .pipe(catchError(this.handleError<any>('agregarDetalle', {})));
  }

  actualizarEstado(idOrden: number, estado: string): Observable<any> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.put<any>(`${this.apiUrl}/${idOrden}/estado`, { estado }, httpOptions(token))
      .pipe(catchError(this.handleError<any>('actualizarEstado', {})));
  }

  eliminarDetalle(idDetalle: number): Observable<any> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.delete<any>(`${this.apiUrl}/detalle/${idDetalle}`, httpOptions(token))
      .pipe(catchError(this.handleError<any>('eliminarDetalle', {})));
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed:`, error);
      return of(result as T);
    };
  }
}