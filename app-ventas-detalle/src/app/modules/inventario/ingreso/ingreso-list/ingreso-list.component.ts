import { CommonModule, DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { Ingreso } from '../../../../interfaces/ingreso.interface';
import { IngresoService } from '../service/ingreso.service';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';

@Component({
  selector: 'app-ingreso-list',
  imports: [CommonModule, DatePipe],
  templateUrl: './ingreso-list.component.html',
  styleUrl: './ingreso-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class IngresoListComponent implements OnInit {
  @Input() public ingresos: Ingreso[] = [];
  public username: string = '';

  constructor(
    private ingresoService: IngresoService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit(): void {
    const user = JSON.parse(sessionStorage.getItem('user') || '{}');
    this.username = user.fullname || user.username || 'Usuario';
  }

  reloadIngresos(): void {
    this.ingresoService.getIngresos().subscribe({
      next: (ingresos) => {
        this.ingresos = ingresos;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });
  }

  crearIngreso() {
    this.router.navigate(['/dashboard/ingreso/add']);
  }

  verDetalle(id: number) {
    this.router.navigate(['/dashboard/ingreso/detalle', id]);
  }

  eliminarIngreso(id: number) {
    Swal.fire({
      title: '¿Estás seguro?',
      text: 'No podrás revertir esto',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar',
    }).then((result) => {
      if (result.isConfirmed) {
        this.ingresoService.deleteIngreso(id).subscribe({
          next: () => {
            Swal.fire('¡Eliminado!', 'Ingreso eliminado correctamente.', 'success');
            this.reloadIngresos();
          },
          error: (err) => console.error(err),
        });
      }
    });
  }
}