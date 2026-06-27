import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AdquisicionService } from '../service/adquisicion.service';
import { OrdenCompraService } from '../service/orden-compra.service';
import { Adquisicion } from '../../../interfaces/adquisicion.interface';
import { OrdenCompra } from '../../../interfaces/orden-compra.interface';

@Component({
  selector: 'app-adquisicion',
  imports: [CommonModule, RouterModule],
  templateUrl: './adquisicion.component.html',
  styleUrl: './adquisicion.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdquisicionComponent implements OnInit {

  adquisiciones: Adquisicion[] = [];
  ordenesAprobadas: OrdenCompra[] = [];
  ordenExpandida: number | null = null;

  constructor(
    private adquisicionService: AdquisicionService,
    private ordenService: OrdenCompraService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.cargarDatos();
  }

  cargarDatos(): void {
    this.adquisicionService.getAll().subscribe(data => {
      this.adquisiciones = data;
      this.cdr.markForCheck();
    });
    this.ordenService.getAll().subscribe(data => {
      this.ordenesAprobadas = data.filter(o => o.estado === 'aprobado');
      this.cdr.markForCheck();
    });
  }

  toggleDetalle(id: number): void {
    this.ordenExpandida = this.ordenExpandida === id ? null : id;
    this.cdr.markForCheck();
  }

  generarNota(orden: OrdenCompra): void {
    this.router.navigate(['/dashboard/compra/nota-compra'], {
      queryParams: { ordenId: orden.id }
    });
  }
}