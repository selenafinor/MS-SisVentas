import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../environments/environment.development';
import { NotaCompra, DetalleNotaCompra } from '../../../interfaces/nota-compra.interface';

function httpOptions(token: string) {
  return {
    headers: new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    })
  };
}

@Injectable({ providedIn: 'root' })
export class NotaCompraService {
  private apiUrl = `${environment.URL_SERVICIOS}/notacompra`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<NotaCompra[]> {
    const token = sessionStorage.getItem('token');
    if (!token) return of([]);
    return this.http.get<NotaCompra[]>(this.apiUrl, httpOptions(token))
      .pipe(catchError(this.handleError<NotaCompra[]>('getAll', [])));
  }

  create(nota: NotaCompra): Observable<NotaCompra> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.post<NotaCompra>(this.apiUrl, nota, httpOptions(token))
      .pipe(catchError(this.handleError<NotaCompra>('create', {})));
  }

  agregarDetalle(idNota: number, detalle: DetalleNotaCompra): Observable<any> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.post<any>(`${this.apiUrl}/${idNota}/detalle`, detalle, httpOptions(token))
      .pipe(catchError(this.handleError<any>('agregarDetalle', {})));
  }

  confirmarStock(idNota: number): Observable<any> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.post<any>(`${this.apiUrl}/${idNota}/confirmar-stock`, {}, httpOptions(token))
      .pipe(catchError(this.handleError<any>('confirmarStock', {})));
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed:`, error);
      return of(result as T);
    };
  }
}