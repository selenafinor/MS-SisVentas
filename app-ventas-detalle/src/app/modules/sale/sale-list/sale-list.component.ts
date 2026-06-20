import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Sale } from '../../../interfaces/sale.interface';
import { SaleService } from '../service/sale.service';

@Component({
  selector: 'app-sale-list',
  imports: [CommonModule, RouterModule],
  templateUrl: './sale-list.component.html',
  styleUrl: './sale-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SaleListComponent implements OnInit {

  ventas = signal<Sale[]>([]);
  cargando = signal<boolean>(false);

  constructor(
    private saleService: SaleService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.cargarVentas();
  }

  cargarVentas(): void {
    this.cargando.set(true);
    this.saleService.getSalesAll().subscribe(data => {
      this.ventas.set(data);
      this.cargando.set(false);
      this.cdr.markForCheck();
    });
  }

  cancelarVenta(id: number): void {
    if (!confirm('¿Está seguro de cancelar esta venta?')) return;
    this.saleService.cancelarVenta(id).subscribe(() => {
      this.cargarVentas();
    });
  }

  getCodigo(id: number): string {
    return `VTA-${String(id).padStart(5, '0')}`;
  }

  getEstadoClass(estado: string): string {
    switch (estado) {
      case 'pagado':    return 'text-green-600 font-semibold';
      case 'activo':    return 'text-blue-600 font-semibold';
      case 'cancelado': return 'text-red-500 font-semibold';
      default:          return '';
    }
  }
}