import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Traspaso, DetalleTraspaso } from '../../../../interfaces/traspaso.interface';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../../environments/environment.development';

const httpOptions = (token: string) => ({
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    Authorization: `Bearer ${token}`,
  }),
});

@Injectable({
  providedIn: 'root',
})
export class TraspasoService {
  private apiUrl = `${environment.URL_SERVICIOS}/traspaso`;
  private detalleUrl = `${environment.URL_SERVICIOS}/DetalleTraspaso`;
  private articuloAlmacenUrl = `${environment.URL_SERVICIOS}/ArticuloAlmacen`;
  private almacenUrl = `${environment.URL_SERVICIOS}/inventario/almacen`;

  constructor(private http: HttpClient) {}

  private getToken(): string | null {
    return sessionStorage.getItem('token');
  }

  getTraspasos(): Observable<Traspaso[]> {
    const token = this.getToken();
    if (!token) return of([]);
    return this.http
      .get<Traspaso[]>(this.apiUrl, httpOptions(token))
      .pipe(catchError(this.handleError('getTraspasos', [])));
  }

  addTraspaso(traspaso: Traspaso): Observable<Traspaso> {
    const token = this.getToken();
    if (!token) return of({} as Traspaso);
    return this.http
      .post<Traspaso>(this.apiUrl, traspaso, httpOptions(token))
      .pipe(catchError(this.handleError('addTraspaso', traspaso)));
  }

  deleteTraspaso(id: number): Observable<any> {
    const token = this.getToken();
    if (!token) return of(null);
    return this.http
      .delete(`${this.apiUrl}/${id}`, httpOptions(token))
      .pipe(catchError(this.handleError('deleteTraspaso')));
  }

  addDetalleTraspaso(detalle: DetalleTraspaso): Observable<DetalleTraspaso> {
    const token = this.getToken();
    if (!token) return of({} as DetalleTraspaso);
    return this.http
      .post<DetalleTraspaso>(this.detalleUrl, detalle, httpOptions(token))
      .pipe(catchError(this.handleError('addDetalleTraspaso', detalle)));
  }

  getArticulosAlmacen(): Observable<any[]> {
    const token = this.getToken();
    if (!token) return of([]);
    return this.http
      .get<any[]>(this.articuloAlmacenUrl, httpOptions(token))
      .pipe(catchError(this.handleError('getArticulosAlmacen', [])));
  }

  getAlmacenes(): Observable<any[]> {
    const token = this.getToken();
    if (!token) return of([]);
    return this.http
      .get<any[]>(this.almacenUrl, httpOptions(token))
      .pipe(catchError(this.handleError('getAlmacenes', [])));
  }
  getTraspasoById(id: number): Observable<any> {
  const token = this.getToken();
  if (!token) return of(null);
  return this.http
    .get<any>(`${this.apiUrl}/${id}`, httpOptions(token))
    .pipe(catchError(this.handleError('getTraspasoById', null)));
}

getDetallesByTraspaso(id: number): Observable<any[]> {
  const token = this.getToken();
  if (!token) return of([]);
  return this.http
    .get<any[]>(`${this.detalleUrl}/traspaso/${id}`, httpOptions(token))
    .pipe(catchError(this.handleError('getDetallesByTraspaso', [])));
}

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      return of(result as T);
    };
  }
}