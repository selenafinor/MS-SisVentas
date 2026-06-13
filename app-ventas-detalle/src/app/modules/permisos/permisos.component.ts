import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PermisoService } from './permiso.service';
import { Permiso } from '../../interfaces/permiso.interface';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-permisos',
  imports: [CommonModule],
  templateUrl: './permisos.component.html',
  styleUrl: './permisos.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PermisosComponent implements OnInit {
  permisos: Permiso[] = [];

  constructor(
    private permisoService: PermisoService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadPermisos();
  }

  loadPermisos(): void {
    this.permisoService.getPermisos().subscribe({
      next: (data) => {
        this.permisos = data;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });
  }

  crearPermiso(): void {
    this.router.navigate(['/dashboard/permisos/add']);
  }

  eliminarPermiso(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: 'No podrás revertir esto',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar',
    }).then((result) => {
      if (result.isConfirmed) {
        this.permisoService.deletePermiso(id).subscribe({
          next: () => {
            Swal.fire('¡Eliminado!', 'Permiso eliminado correctamente.', 'success');
            this.loadPermisos();
          },
          error: (err) => console.error(err),
        });
      }
    });
  }
}