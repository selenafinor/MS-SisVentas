import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../environments/environment.development';
import { Adquisicion, DetalleAdquisicion } from '../../../interfaces/adquisicion.interface';

function httpOptions(token: string) {
  return {
    headers: new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    })
  };
}

@Injectable({ providedIn: 'root' })
export class AdquisicionService {
  private apiUrl = `${environment.URL_SERVICIOS}/adquisicion`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Adquisicion[]> {
    const token = sessionStorage.getItem('token');
    if (!token) return of([]);
    return this.http.get<Adquisicion[]>(this.apiUrl, httpOptions(token))
      .pipe(catchError(this.handleError<Adquisicion[]>('getAll', [])));
  }

  create(adquisicion: Adquisicion): Observable<Adquisicion> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.post<Adquisicion>(this.apiUrl, adquisicion, httpOptions(token))
      .pipe(catchError(this.handleError<Adquisicion>('create', {})));
  }

  agregarDetalle(idAdquisicion: number, detalle: DetalleAdquisicion): Observable<any> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.post<any>(`${this.apiUrl}/${idAdquisicion}/detalle`, detalle, httpOptions(token))
      .pipe(catchError(this.handleError<any>('agregarDetalle', {})));
  }

  confirmarStock(idAdquisicion: number): Observable<any> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({});
    return this.http.post<any>(`${this.apiUrl}/${idAdquisicion}/confirmar-stock`, {}, httpOptions(token))
      .pipe(catchError(this.handleError<any>('confirmarStock', {})));
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed:`, error);
      return of(result as T);
    };
  }
}