import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input } from '@angular/core';
import { User } from '../../../interfaces/user.interface';
import { CommonModule } from '@angular/common';
import { UsuarioService } from '../usuario.service';
import Swal from 'sweetalert2';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-usuario-list',
  imports: [CommonModule, RouterModule],
  templateUrl: './usuario-list.component.html',
  styleUrl: './usuario-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UsuarioListComponent {
  @Input() public users: User[] = [];

  constructor(
    private usuarioService: UsuarioService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  createUser() {
    this.router.navigate(['/dashboard/user/add']);
  }

  

  reloadUsers(): void {
    this.usuarioService.getUsuarios().subscribe({
      next: (users) => {
        this.users = users;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });
  }
  gestionarUsuario(userId: number) {
  this.router.navigate(['/dashboard/user/gestionar', userId]);
}
cambiarEstado(user: User): void {
  const nuevoEstado = user.estado === 'activo' ? 'inactivo' : 'activo';
  const mensaje = nuevoEstado === 'inactivo' ? 'deshabilitar' : 'habilitar';

  Swal.fire({
    title: `¿${mensaje} este usuario?`,
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: `Sí, ${mensaje}`,
    cancelButtonText: 'Cancelar',
  }).then((result) => {
    if (result.isConfirmed) {
      const usuarioActualizado = {
        userId: user.userId,
        fullname: user.fullname,
        username: user.username,
        correo: user.correo,
        telefono: user.telefono,
        estado: nuevoEstado,
        password: ''
      };
      this.usuarioService.updateUsuario(user.userId!, usuarioActualizado).subscribe({
        next: () => {
          Swal.fire('¡Listo!', `Usuario ${mensaje}do correctamente.`, 'success');
          this.reloadUsers();
        },
        error: (err) => console.error(err),
      });
    }
  });
}
desbloquearUsuario(user: User): void {
  Swal.fire({
    title: '¿Desbloquear este usuario?',
    text: 'El usuario podrá intentar iniciar sesión de nuevo inmediatamente.',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Sí, desbloquear',
    cancelButtonText: 'Cancelar',
  }).then((result) => {
    if (result.isConfirmed) {
      this.usuarioService.desbloquearUsuario(user.userId!).subscribe({
        next: () => {
          Swal.fire('¡Listo!', 'Usuario desbloqueado correctamente.', 'success');
          this.reloadUsers();
        },
        error: (err) => console.error(err),
      });
    }
  });
}

estaBloqueado(user: User): boolean {
  if (!user.bloqueadoHasta) return false;
  return new Date(user.bloqueadoHasta) > new Date();
}
}