import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Egreso } from '../../../interfaces/egreso.interface';
import { EgresoService } from './service/egreso.service';
import { EgresoListComponent } from './egreso-list/egreso-list.component';

@Component({
  selector: 'app-egreso',
  imports: [CommonModule, EgresoListComponent],
  templateUrl: './egreso.component.html',
  styleUrl: './egreso.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EgresoComponent implements OnInit {
  egresos: Egreso[] = [];
  errorMessage: string | null = null;

  constructor(
    private egresoService: EgresoService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadEgresos();
  }

  loadEgresos(): void {
    this.egresoService.getEgresos().subscribe({
      next: (data) => {
        this.egresos = data;
        this.cdr.markForCheck();
      },
      error: () => {
        this.errorMessage = 'Error al cargar egresos';
        this.cdr.markForCheck();
      }
    });
  }
}