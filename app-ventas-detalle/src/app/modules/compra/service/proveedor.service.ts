import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../environments/environment.development';
import { Proveedor } from '../../../interfaces/proveedor.interface';

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
export class ProveedorService {
  private apiUrl = `${environment.URL_SERVICIOS}/proveedor`;

  constructor(private http: HttpClient) { }

  // Obtener todos los proveedores
  getProveedorAll(): Observable<Proveedor[]> {
    const token = sessionStorage.getItem('token');
    if (!token) return of([]);
    return this.http.get<Proveedor[]>(this.apiUrl, httpOptions(token))
      .pipe(catchError(this.handleError<Proveedor[]>('getProveedorAll', [])));
  }

  // Obtener proveedor por id
  getProveedorById(id: number): Observable<Proveedor> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.get<Proveedor>(`${this.apiUrl}/${id}`, httpOptions(token))
      .pipe(catchError(this.handleError<Proveedor>('getProveedorById', {})));
  }

  // Crear proveedor
  createProveedor(proveedor: Proveedor): Observable<Proveedor> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.post<Proveedor>(this.apiUrl, proveedor, httpOptions(token))
      .pipe(catchError(this.handleError<Proveedor>('createProveedor', {})));
  }

  // Actualizar proveedor
  updateProveedor(proveedor: Proveedor): Observable<Proveedor> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.put<Proveedor>(`${this.apiUrl}/${proveedor.id}`, proveedor, httpOptions(token))
      .pipe(catchError(this.handleError<Proveedor>('updateProveedor', {})));
  }

  // Habilitar/deshabilitar proveedor
  toggleEstado(id: number): Observable<any> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.put<any>(`${this.apiUrl}/${id}/toggle`, {}, httpOptions(token))
      .pipe(catchError(this.handleError<any>('toggleEstado', {})));
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed:`, error);
      return of(result as T);
    };
  }
}