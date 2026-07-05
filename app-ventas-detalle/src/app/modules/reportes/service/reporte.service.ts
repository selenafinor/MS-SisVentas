import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  ReporteVentasResponse,
  ReporteComprasResponse,
  ReporteInventarioResponse,
  ReporteMovimientosResponse,
  EnviarCorreoRequest,
  DashboardResponse
} from '../../../interfaces/reporte.interface';

const httpOptions = (token: string) => ({
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    Authorization: `Bearer ${token}`,
  }),
});

const httpOptionsBlob = (token: string) => ({
  headers: new HttpHeaders({
    Authorization: `Bearer ${token}`,
  }),
  responseType: 'blob' as 'json'
});

@Injectable({
  providedIn: 'root'
})
export class ReporteService {
  private baseUrl = 'http://localhost:5000/api/reporte';

  constructor(private http: HttpClient) {}

  private getToken(): string {
    return sessionStorage.getItem('token') || '';
  }

  getDashboard(): Observable<DashboardResponse> {
    return this.http.get<DashboardResponse>(`${this.baseUrl}/dashboard`, httpOptions(this.getToken()));
  }

  getReporteVentas(filtros: {
    desde?: string;
    hasta?: string;
    estado?: string;
    tipoPago?: string;
  }): Observable<ReporteVentasResponse> {
    let params = this.construirParams(filtros);
    return this.http.get<ReporteVentasResponse>(`${this.baseUrl}/ventas`, {
      ...httpOptions(this.getToken()),
      params
    });
  }

  getReporteCompras(filtros: {
    desde?: string;
    hasta?: string;
    estado?: string;
  }): Observable<ReporteComprasResponse> {
    let params = this.construirParams(filtros);
    return this.http.get<ReporteComprasResponse>(`${this.baseUrl}/compras`, {
      ...httpOptions(this.getToken()),
      params
    });
  }

  getReporteInventario(): Observable<ReporteInventarioResponse> {
    return this.http.get<ReporteInventarioResponse>(`${this.baseUrl}/inventario`, httpOptions(this.getToken()));
  }

  getReporteMovimientos(filtros: {
    desde?: string;
    hasta?: string;
    tipo?: string;
  }): Observable<ReporteMovimientosResponse> {
    let params = this.construirParams(filtros);
    return this.http.get<ReporteMovimientosResponse>(`${this.baseUrl}/movimientos`, {
      ...httpOptions(this.getToken()),
      params
    });
  }

  descargarPdfVentas(filtros: {
    desde?: string;
    hasta?: string;
    estado?: string;
    tipoPago?: string;
  }): Observable<Blob> {
    let params = this.construirParams(filtros);
    return this.http.get(`${this.baseUrl}/ventas/pdf`, {
      ...httpOptionsBlob(this.getToken()),
      params
    }) as unknown as Observable<Blob>;
  }

  descargarPdfCompras(filtros: {
    desde?: string;
    hasta?: string;
    estado?: string;
  }): Observable<Blob> {
    let params = this.construirParams(filtros);
    return this.http.get(`${this.baseUrl}/compras/pdf`, {
      ...httpOptionsBlob(this.getToken()),
      params
    }) as unknown as Observable<Blob>;
  }

  descargarPdfInventario(): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/inventario/pdf`, httpOptionsBlob(this.getToken())) as unknown as Observable<Blob>;
  }

  descargarPdfMovimientos(filtros: {
    desde?: string;
    hasta?: string;
  }): Observable<Blob> {
    let params = this.construirParams(filtros);
    return this.http.get(`${this.baseUrl}/movimientos/pdf`, {
      ...httpOptionsBlob(this.getToken()),
      params
    }) as unknown as Observable<Blob>;
  }

  enviarPorCorreo(request: EnviarCorreoRequest): Observable<{ mensaje: string }> {
    return this.http.post<{ mensaje: string }>(`${this.baseUrl}/enviar-correo`, request, httpOptions(this.getToken()));
  }

  private construirParams(filtros: { [key: string]: string | undefined }): any {
    const params: any = {};
    Object.keys(filtros).forEach(key => {
      if (filtros[key]) {
        params[key] = filtros[key];
      }
    });
    return params;
  }
}