import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../environments/environment.development';
import { CatalogoProveedor } from '../../../interfaces/catalogo-proveedor.interface';

function httpOptions(token: string) {
  return {
    headers: new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    })
  };
}

@Injectable({ providedIn: 'root' })
export class CatalogoProveedorService {
  private apiUrl = `${environment.URL_SERVICIOS}/catalogoproveedor`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<CatalogoProveedor[]> {
    const token = sessionStorage.getItem('token');
    if (!token) return of([]);
    return this.http.get<CatalogoProveedor[]>(this.apiUrl, httpOptions(token))
      .pipe(catchError(this.handleError<CatalogoProveedor[]>('getAll', [])));
  }

  getByProveedor(proveedorId: number): Observable<CatalogoProveedor[]> {
    const token = sessionStorage.getItem('token');
    if (!token) return of([]);
    return this.http.get<CatalogoProveedor[]>(`${this.apiUrl}/proveedor/${proveedorId}`, httpOptions(token))
      .pipe(catchError(this.handleError<CatalogoProveedor[]>('getByProveedor', [])));
  }

  getByProducto(productoId: number): Observable<CatalogoProveedor[]> {
    const token = sessionStorage.getItem('token');
    if (!token) return of([]);
    return this.http.get<CatalogoProveedor[]>(`${this.apiUrl}/producto/${productoId}`, httpOptions(token))
      .pipe(catchError(this.handleError<CatalogoProveedor[]>('getByProducto', [])));
  }

  create(item: CatalogoProveedor): Observable<CatalogoProveedor> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.post<CatalogoProveedor>(this.apiUrl, item, httpOptions(token))
      .pipe(catchError(this.handleError<CatalogoProveedor>('create', {})));
  }

  update(item: CatalogoProveedor): Observable<any> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.put<any>(`${this.apiUrl}/${item.id}`, item, httpOptions(token))
      .pipe(catchError(this.handleError<any>('update', {})));
  }

  delete(id: number): Observable<any> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.delete<any>(`${this.apiUrl}/${id}`, httpOptions(token))
      .pipe(catchError(this.handleError<any>('delete', {})));
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed:`, error);
      return of(result as T);
    };
  }
}