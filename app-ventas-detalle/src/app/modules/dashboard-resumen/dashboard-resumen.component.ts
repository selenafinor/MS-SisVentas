import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReporteService } from '../reportes/service/reporte.service';
import { DashboardResponse } from '../../interfaces/reporte.interface';

@Component({
  selector: 'app-dashboard-resumen',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard-resumen.component.html',
  styleUrl: './dashboard-resumen.component.css'
})
export class DashboardResumenComponent implements OnInit {
  dashboard: DashboardResponse | null = null;
  cargando = false;
  error = false;

  private userPermissions: string[] = [];

  constructor(
    private reporteService: ReporteService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.cargarPermisos();
    this.cargarDashboard();
  }

  private cargarPermisos(): void {
    const roles = JSON.parse(sessionStorage.getItem('roles') || '[]');
    const permisos = roles.flatMap((rol: any) => rol.permisos || []);
    this.userPermissions = permisos.map((p: any) => p.nombre_Permiso);
  }

  hasPermission(permissionName: string): boolean {
    return this.userPermissions.includes(permissionName);
  }

  cargarDashboard(): void {
    this.cargando = true;
    this.error = false;

    this.reporteService.getDashboard().subscribe({
      next: (res) => {
        this.dashboard = res;
        this.cargando = false;
        this.cdr.markForCheck();
      },
      error: (err) => {
        console.error('Error cargando dashboard:', err);
        this.cargando = false;
        this.error = true;
        this.cdr.markForCheck();
      }
    });
  }

  getNombreCliente(cliente: any): string {
    if (!cliente) return '—';
    return `${cliente.nombre || ''} ${cliente.paterno || ''}`.trim() || '—';
  }
}