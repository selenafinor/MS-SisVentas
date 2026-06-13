import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Almacen } from '../../../../interfaces/almacen.interface';
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
export class AlmacenService {
  private apiUrl = `${environment.URL_SERVICIOS}/inventario/almacen`;

  constructor(private http: HttpClient) {}

  getAlmacenes(): Observable<Almacen[]> {
    const token = sessionStorage.getItem('token');
    if (token) {
      return this.http
        .get<Almacen[]>(this.apiUrl, httpOptions(token))
        .pipe(catchError(this.handleError('getAlmacen', [])));
    } else {
      return of([]);
    }
  }

  getAlmacenById(id: number): Observable<Almacen> {
    const token = sessionStorage.getItem('token');
    return this.http.get<Almacen>(`${this.apiUrl}/${id}`, httpOptions(token!));
  }

  addAlmacen(almacen: Almacen): Observable<Almacen> {
    const token = sessionStorage.getItem('token');
    if (token) {
      if (!almacen.nombre) {
        console.error('Faltan datos necesarios para crear el almacen');
        return of({} as Almacen);
      }
      return this.http
        .post<Almacen>(this.apiUrl, almacen, httpOptions(token))
        .pipe(catchError(this.handleError('addAlmacen', almacen)));
    } else {
      return of({} as Almacen);
    }
  }

  updateAlmacen(id: number, almacen: Almacen): Observable<void> {
    const token = sessionStorage.getItem('token');
    return this.http.put<void>(`${this.apiUrl}/${id}`, almacen, httpOptions(token!));
  }

  deleteAlmacen(id: number): Observable<any> {
    const token = sessionStorage.getItem('token');
    if (token) {
      return this.http
        .delete(`${this.apiUrl}/${id}`, httpOptions(token))
        .pipe(catchError(this.handleError('deleteAlmacen')));
    } else {
      return of(null);
    }
  }
  getArticulosAlmacen(): Observable<any[]> {
  const token = sessionStorage.getItem('token');
  if (!token) return of([]);
  return this.http
    .get<any[]>(`${environment.URL_SERVICIOS}/ArticuloAlmacen`, httpOptions(token))
    .pipe(catchError(this.handleError('getArticulosAlmacen', [])));
}

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      return of(result as T);
    };
  }
}
