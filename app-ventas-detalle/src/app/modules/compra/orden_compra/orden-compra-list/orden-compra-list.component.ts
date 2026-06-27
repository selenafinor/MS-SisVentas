import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { OrdenCompraService } from '../../service/orden-compra.service';
import { OrdenCompra } from '../../../../interfaces/orden-compra.interface';

@Component({
  selector: 'app-orden-compra-list',
  imports: [CommonModule, RouterModule],
  templateUrl: './orden-compra-list.component.html',
  styleUrl: './orden-compra-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OrdenCompraListComponent implements OnInit {
  ordenes: OrdenCompra[] = [];
  errorMessage: string = '';
  ordenExpandida: number | null = null;

  constructor(
    private ordenService: OrdenCompraService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadOrdenes();
  }

  loadOrdenes(): void {
    this.ordenService.getAll().subscribe({
      next: (data) => {
        this.ordenes = Array.isArray(data) ? data : [];
        this.errorMessage = this.ordenes.length === 0 ? 'No hay órdenes registradas.' : '';
        this.cdr.markForCheck();
      },
      error: () => {
        this.errorMessage = 'Error al cargar las órdenes.';
        this.cdr.markForCheck();
      }
    });
  }

  toggleDetalle(id: number): void {
    this.ordenExpandida = this.ordenExpandida === id ? null : id;
    this.cdr.markForCheck();
  }

  aprobarOrden(id: number): void {
    if (!confirm('¿Aprobar esta orden? Una vez aprobada estará disponible para recepción.')) return;
    this.ordenService.actualizarEstado(id, 'aprobado').subscribe(() => {
      this.loadOrdenes();
    });
  }

  getEstadoClass(estado: string): string {
    switch (estado) {
      case 'pendiente': return 'text-yellow-400 font-semibold';
      case 'aprobado':  return 'text-green-400 font-semibold';
      case 'recibido':  return 'text-blue-400 font-semibold';
      case 'cancelado': return 'text-red-400 font-semibold';
      default:          return 'text-gray-400';
    }
  }
}