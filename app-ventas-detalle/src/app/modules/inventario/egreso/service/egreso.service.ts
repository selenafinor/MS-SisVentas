import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Egreso, DetalleEgreso } from '../../../../interfaces/egreso.interface';
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
export class EgresoService {
  private apiUrl = `${environment.URL_SERVICIOS}/egreso`;
  private detalleUrl = `${environment.URL_SERVICIOS}/DetalleEgreso`;
  private articuloAlmacenUrl = `${environment.URL_SERVICIOS}/ArticuloAlmacen`;

  constructor(private http: HttpClient) {}

  private getToken(): string | null {
    return sessionStorage.getItem('token');
  }

  getEgresos(): Observable<Egreso[]> {
    const token = this.getToken();
    if (!token) return of([]);
    return this.http
      .get<Egreso[]>(this.apiUrl, httpOptions(token))
      .pipe(catchError(this.handleError('getEgresos', [])));
  }

  addEgreso(egreso: Egreso): Observable<Egreso> {
    const token = this.getToken();
    if (!token) return of({} as Egreso);
    return this.http
      .post<Egreso>(this.apiUrl, egreso, httpOptions(token))
      .pipe(catchError(this.handleError('addEgreso', egreso)));
  }

  deleteEgreso(id: number): Observable<any> {
    const token = this.getToken();
    if (!token) return of(null);
    return this.http
      .delete(`${this.apiUrl}/${id}`, httpOptions(token))
      .pipe(catchError(this.handleError('deleteEgreso')));
  }

  addDetalleEgreso(detalle: DetalleEgreso): Observable<DetalleEgreso> {
    const token = this.getToken();
    if (!token) return of({} as DetalleEgreso);
    return this.http
      .post<DetalleEgreso>(this.detalleUrl, detalle, httpOptions(token))
      .pipe(catchError(this.handleError('addDetalleEgreso', detalle)));
  }

  getArticulosAlmacen(): Observable<any[]> {
    const token = this.getToken();
    if (!token) return of([]);
    return this.http
      .get<any[]>(this.articuloAlmacenUrl, httpOptions(token))
      .pipe(catchError(this.handleError('getArticulosAlmacen', [])));
  }
  getEgresoById(id: number): Observable<any> {
  const token = this.getToken();
  if (!token) return of(null);
  return this.http
    .get<any>(`${this.apiUrl}/${id}`, httpOptions(token))
    .pipe(catchError(this.handleError('getEgresoById', null)));
}

getDetallesByEgreso(id: number): Observable<any[]> {
  const token = this.getToken();
  if (!token) return of([]);
  return this.http
    .get<any[]>(`${this.detalleUrl}/egreso/${id}`, httpOptions(token))
    .pipe(catchError(this.handleError('getDetallesByEgreso', [])));
}

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      return of(result as T);
    };
  }
}