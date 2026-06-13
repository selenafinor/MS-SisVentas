import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { Almacen } from '../../../../interfaces/almacen.interface';
import { AlmacenService } from '../service/almacen.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-almacen-list',
  imports: [CommonModule],
  templateUrl: './almacen-list.component.html',
  styleUrl: './almacen-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AlmacenListComponent implements OnInit {
  @Input() public almacenes: Almacen[] = [];
  public articulosAlmacen: any[] = [];
  public almacenExpandido: number | null = null;

  constructor(
    private almacenService: AlmacenService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.almacenService.getArticulosAlmacen().subscribe({
      next: (data) => {
        this.articulosAlmacen = data;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });
  }

  getArticulosPorAlmacen(almacenId: number): any[] {
    return this.articulosAlmacen.filter(aa => aa.id_Almacen === almacenId);
  }

  getStockTotal(almacenId: number): number {
    return this.getArticulosPorAlmacen(almacenId)
      .reduce((sum, aa) => sum + (aa.stock || 0), 0);
  }

  getPorcentaje(almacenId: number, cantidadMax: number): number {
    if (!cantidadMax) return 0;
    return Math.min(Math.round((this.getStockTotal(almacenId) / cantidadMax) * 100), 100);
  }

  toggleStock(almacenId: number): void {
    this.almacenExpandido = this.almacenExpandido === almacenId ? null : almacenId;
  }

  reloadAlmacenes(): void {
    this.almacenService.getAlmacenes().subscribe({
      next: (almacenes) => {
        this.almacenes = almacenes;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });
  }

  crearAlmacen() {
    this.router.navigate(['/dashboard/almacen/add']);
  }

  editarAlmacen(id: number) {
    this.router.navigate(['/dashboard/almacen/edit', id]);
  }
}