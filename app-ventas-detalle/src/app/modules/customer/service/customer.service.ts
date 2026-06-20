import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../environments/environment.development';
import { Customer } from '../../../interfaces/customer.interface';

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
export class CustomerService {
  private apiUrl = `${environment.URL_SERVICIOS}/cliente`;

  constructor(private http: HttpClient) { }

  // Obtener todos los clientes
  getCustomerAll(): Observable<Customer[]> {
    const token = sessionStorage.getItem('token');
    if (!token) return of([]);
    return this.http.get<Customer[]>(this.apiUrl, httpOptions(token))
      .pipe(catchError(this.handleError<Customer[]>('getCustomerAll', [])));
  }

  // Obtener cliente por id
  getCustomerById(id: number): Observable<Customer> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.get<Customer>(`${this.apiUrl}/${id}`, httpOptions(token))
      .pipe(catchError(this.handleError<Customer>('getCustomerById', {})));
  }

  // Buscar clientes por nombre o NIT
  buscarClientes(termino: string): Observable<Customer[]> {
    const token = sessionStorage.getItem('token');
    if (!token) return of([]);
    return this.http.get<Customer[]>(`${this.apiUrl}/buscar/${termino}`, httpOptions(token))
      .pipe(catchError(this.handleError<Customer[]>('buscarClientes', [])));
  }

  // Crear cliente
  createCustomer(customer: Customer): Observable<Customer> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.post<Customer>(this.apiUrl, customer, httpOptions(token))
      .pipe(catchError(this.handleError<Customer>('createCustomer', {})));
  }

  // Actualizar cliente
  updateCustomer(customer: Customer): Observable<Customer> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.put<Customer>(`${this.apiUrl}/${customer.id}`, customer, httpOptions(token))
      .pipe(catchError(this.handleError<Customer>('updateCustomer', {})));
  }

  // Habilitar/deshabilitar cliente
  toggleEstado(id: number): Observable<any> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.put<any>(`${this.apiUrl}/${id}/toggle`, {}, httpOptions(token))
      .pipe(catchError(this.handleError<any>('toggleEstado', {})));
  }

  // Eliminar cliente
  deleteCustomer(id: number): Observable<any> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.delete<any>(`${this.apiUrl}/${id}`, httpOptions(token))
      .pipe(catchError(this.handleError<any>('deleteCustomer', {})));
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed:`, error);
      return of(result as T);
    };
  }
}