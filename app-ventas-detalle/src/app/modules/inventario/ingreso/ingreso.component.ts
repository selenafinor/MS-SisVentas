import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Ingreso } from '../../../interfaces/ingreso.interface';
import { IngresoService } from './service/ingreso.service';
import { IngresoListComponent } from './ingreso-list/ingreso-list.component';

@Component({
  selector: 'app-ingreso',
  imports: [CommonModule, IngresoListComponent],
  templateUrl: './ingreso.component.html',
  styleUrl: './ingreso.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class IngresoComponent implements OnInit {
  ingresos: Ingreso[] = [];
  errorMessage: string | null = null;

  constructor(
    private ingresoService: IngresoService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadIngresos();
  }

  loadIngresos(): void {
    this.ingresoService.getIngresos().subscribe({
      next: (data) => {
        this.ingresos = data;
        this.cdr.markForCheck();
      },
      error: () => {
        this.errorMessage = 'Error al cargar ingresos';
        this.cdr.markForCheck();
      }
    });
  }
}