import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ProveedorService } from '../service/proveedor.service';
import { Proveedor } from '../../../interfaces/proveedor.interface';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-proveedor-list',
  imports: [CommonModule, RouterModule],
  templateUrl: './proveedor-list.component.html',
  styleUrl: './proveedor-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProveedorListComponent implements OnInit {
  proveedores: Proveedor[] = [];
  errorMessage: string = '';

  constructor(
    private proveedorService: ProveedorService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadProveedores();
  }

  loadProveedores(): void {
    this.proveedorService.getProveedorAll().subscribe({
      next: (data) => {
        if (Array.isArray(data) && data.length > 0) {
          this.proveedores = data;
          this.errorMessage = '';
        } else {
          this.proveedores = [];
          this.errorMessage = 'No se encontraron proveedores disponibles.';
        }
        this.cdr.markForCheck();
      },
      error: () => {
        this.errorMessage = 'Error al cargar los proveedores.';
        this.proveedores = [];
        this.cdr.markForCheck();
      }
    });
  }

  toggleEstado(id: number): void {
    this.proveedorService.toggleEstado(id).subscribe(() => {
      this.loadProveedores();
    });
  }
}