import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { RolService } from '../rol.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-rol-list',
  imports: [CommonModule, RouterModule],
  templateUrl: './rol-list.component.html',
  styleUrl: './rol-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RolListComponent {
  @Input() public roles: any[] = [];
  @Output() recargar = new EventEmitter<void>();

  constructor(
    private rolesService: RolService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  cambiarEstado(rol: any): void {
    const nuevoEstado = rol.estado === 'activo' ? 'inactivo' : 'activo';
    const accion = nuevoEstado === 'inactivo' ? 'desactivar' : 'activar';

    Swal.fire({
      title: `¿${accion} este rol?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: `Sí, ${accion}`,
      cancelButtonText: 'Cancelar',
    }).then((result) => {
      if (result.isConfirmed) {
        const rolActualizado = {
          iD_Rol: rol.iD_Rol,
          nombre_Rol: rol.nombre_Rol,
          descripcion: rol.descripcion,
          estado: nuevoEstado
        };
        this.rolesService.updateRol(rol.iD_Rol, rolActualizado).subscribe({
          next: () => {
            rol.estado = nuevoEstado;
            this.cdr.markForCheck();
            Swal.fire('¡Listo!', `Rol ${accion}do correctamente.`, 'success');
          },
          error: (err) => console.error(err),
        });
      }
    });
  }

  asignarPermiso(rol: any): void {
  console.log('Rol:', rol);
  this.router.navigate(['/dashboard/roles/rolpermiso', rol.iD_Rol]);
}

  reloadRoles(): void {
    this.recargar.emit();
  }

  createRole(): void {
    this.router.navigate(['/dashboard/roles/add']);
  }
}