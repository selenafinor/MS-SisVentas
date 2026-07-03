import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  ReporteVentasResponse,
  ReporteComprasResponse,
  ReporteInventarioResponse,
  ReporteMovimientosResponse,
  EnviarCorreoRequest
} from '../../../interfaces/reporte.interface';

@Injectable({
  providedIn: 'root'
})
export class ReporteService {
  private baseUrl = 'http://localhost:5000/api/reporte';

  constructor(private http: HttpClient) {}

  getReporteVentas(filtros: {
    desde?: string;
    hasta?: string;
    estado?: string;
    tipoPago?: string;
  }): Observable<ReporteVentasResponse> {
    let params = this.construirParams(filtros);
    return this.http.get<ReporteVentasResponse>(`${this.baseUrl}/ventas`, { params });
  }

  getReporteCompras(filtros: {
    desde?: string;
    hasta?: string;
    estado?: string;
  }): Observable<ReporteComprasResponse> {
    let params = this.construirParams(filtros);
    return this.http.get<ReporteComprasResponse>(`${this.baseUrl}/compras`, { params });
  }

  getReporteInventario(): Observable<ReporteInventarioResponse> {
    return this.http.get<ReporteInventarioResponse>(`${this.baseUrl}/inventario`);
  }

  getReporteMovimientos(filtros: {
    desde?: string;
    hasta?: string;
    tipo?: string;
  }): Observable<ReporteMovimientosResponse> {
    let params = this.construirParams(filtros);
    return this.http.get<ReporteMovimientosResponse>(`${this.baseUrl}/movimientos`, { params });
  }

  descargarPdfVentas(filtros: {
    desde?: string;
    hasta?: string;
    estado?: string;
    tipoPago?: string;
  }): Observable<Blob> {
    let params = this.construirParams(filtros);
    return this.http.get(`${this.baseUrl}/ventas/pdf`, { params, responseType: 'blob' });
  }

  descargarPdfCompras(filtros: {
    desde?: string;
    hasta?: string;
    estado?: string;
  }): Observable<Blob> {
    let params = this.construirParams(filtros);
    return this.http.get(`${this.baseUrl}/compras/pdf`, { params, responseType: 'blob' });
  }

  descargarPdfInventario(): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/inventario/pdf`, { responseType: 'blob' });
  }

  descargarPdfMovimientos(filtros: {
    desde?: string;
    hasta?: string;
  }): Observable<Blob> {
    let params = this.construirParams(filtros);
    return this.http.get(`${this.baseUrl}/movimientos/pdf`, { params, responseType: 'blob' });
  }

  enviarPorCorreo(request: EnviarCorreoRequest): Observable<{ mensaje: string }> {
    return this.http.post<{ mensaje: string }>(`${this.baseUrl}/enviar-correo`, request);
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