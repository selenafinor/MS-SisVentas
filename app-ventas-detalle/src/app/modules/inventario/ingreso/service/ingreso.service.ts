import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Ingreso, DetalleIngreso } from '../../../../interfaces/ingreso.interface';
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
export class IngresoService {
  private apiUrl = `${environment.URL_SERVICIOS}/ingreso`;
  private detalleUrl = `${environment.URL_SERVICIOS}/DetalleIngreso`;
  private articuloAlmacenUrl = `${environment.URL_SERVICIOS}/ArticuloAlmacen`;

  constructor(private http: HttpClient) {}

  private getToken(): string | null {
    return sessionStorage.getItem('token');
  }

  getIngresos(): Observable<Ingreso[]> {
    const token = this.getToken();
    if (!token) return of([]);
    return this.http
      .get<Ingreso[]>(this.apiUrl, httpOptions(token))
      .pipe(catchError(this.handleError('getIngresos', [])));
  }

  addIngreso(ingreso: Ingreso): Observable<Ingreso> {
    const token = this.getToken();
    if (!token) return of({} as Ingreso);
    return this.http
      .post<Ingreso>(this.apiUrl, ingreso, httpOptions(token))
      .pipe(catchError(this.handleError('addIngreso', ingreso)));
  }

  deleteIngreso(id: number): Observable<any> {
    const token = this.getToken();
    if (!token) return of(null);
    return this.http
      .delete(`${this.apiUrl}/${id}`, httpOptions(token))
      .pipe(catchError(this.handleError('deleteIngreso')));
  }

  addDetalleIngreso(detalle: DetalleIngreso): Observable<DetalleIngreso> {
    const token = this.getToken();
    if (!token) return of({} as DetalleIngreso);
    return this.http
      .post<DetalleIngreso>(this.detalleUrl, detalle, httpOptions(token))
      .pipe(catchError(this.handleError('addDetalleIngreso', detalle)));
  }

  getArticulosAlmacen(): Observable<any[]> {
    const token = this.getToken();
    if (!token) return of([]);
    return this.http
      .get<any[]>(this.articuloAlmacenUrl, httpOptions(token))
      .pipe(catchError(this.handleError('getArticulosAlmacen', [])));
  }
getIngresoById(id: number): Observable<any> {
  const token = this.getToken();
  if (!token) return of(null);
  return this.http
    .get<any>(`${this.apiUrl}/${id}`, httpOptions(token))
    .pipe(catchError(this.handleError('getIngresoById', null)));
}

getDetallesByIngreso(id: number): Observable<any[]> {
  const token = this.getToken();
  if (!token) return of([]);
  return this.http
    .get<any[]>(`${this.detalleUrl}/ingreso/${id}`, httpOptions(token))
    .pipe(catchError(this.handleError('getDetallesByIngreso', [])));
}
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      return of(result as T);
    };
  }
}