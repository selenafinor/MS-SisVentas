import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { forkJoin, of } from 'rxjs';
import { switchMap, catchError } from 'rxjs/operators';
import { User } from '../../../interfaces/user.interface';
import { Rol } from '../../../interfaces/rol.interface';
import { UsuarioService } from '../../usuario/usuario.service';
import { RolService } from '../../rol/rol.service';
import { RolPermisoUsuario } from '../../../interfaces/rol-permiso-usuario.interface';

@Component({
  selector: 'app-roles-permisos-usuario-add',
  imports: [
    CommonModule, RouterModule, FormsModule
  ],
  templateUrl: './roles-permisos-usuario-add.component.html',
  styleUrl: './roles-permisos-usuario-add.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RolesPermisosUsuarioAddComponent implements OnInit {
  user: User | undefined;
  userId!: number;

  selectedUser: number | null = null; // Usuario seleccionado
  selectedRoles: any[] = []; // Roles seleccionados
  asignaciones: any[] = []; // Asignaciones de usuario-rol

  roles: Rol[] = [];
  constructor(
    private usuarioService: UsuarioService,
    private rolService: RolService,
    private cdr: ChangeDetectorRef,
    private router: Router, // Inyectar Router
    private route: ActivatedRoute
  ) {}
  ngOnInit(): void {
    this.userId = +this.route.snapshot.paramMap.get('id')!;
    this.loadUsuario();
    this.loadRoles();
  }

  loadUsuario(): void {
    this.usuarioService.getUsuarioById(this.userId).subscribe({
      next: (usuarios) => {
        this.user = usuarios;
        console.log('Usuarios cargados:', this.user);
        this.cdr.markForCheck();
      },
      error: (err) => {
        console.error(err);
        this.cdr.markForCheck();
      },
    });
  }

  loadRoles(): void {
    this.rolService.getRoles().subscribe({
      next: (roles) => {
        this.roles = roles;
        console.log('Roles cargados:', roles);
        this.cdr.markForCheck();
      },
      error: (err) => {
        console.error(err);
        this.cdr.markForCheck();
      },
    });
  }

  asignarRoles() {
    if (this.selectedUser && this.selectedRoles.length > 0) {
      this.selectedRoles.forEach((rol) => {
        this.asignaciones.push({
          userId: this.selectedUser,
          rol,
        });
      });

      console.log('Asignaciones:', this.asignaciones);
    } else {
      alert('Selecciona un usuario y al menos un rol.');
    }
  }

  transformarDatos(userId: number, data: any[]): any {
    return {
      userId: userId,
      rolpermiso: data
        // .filter(item => item.userId === userId) // Filtramos por el userId específico
        .flatMap((item) => item.rol.rolPermisos) // Extraemos los permisos
        .map((permiso) => ({ iD_Rol_Permiso: permiso.iD_Rol_Permiso })), // Formateamos
    };
  }

  save(): void {
    // 🔹 1. Construcción del objeto para la API
    const rolData = this.transformarDatos(this.userId, this.asignaciones);

    if (rolData.rolpermiso.length === 0) {
      alert('Selecciona un usuario y al menos un rol.');
      return;
    }

    // 🔹 2. Antes de asignar los roles nuevos, hay que borrar las
    // asignaciones (RolPermisoUsuario) que el usuario ya tenía.
    // Sin este paso, los roles nuevos se sumaban a los viejos en vez
    // de reemplazarlos (por eso un usuario terminaba con 2 roles).
    this.rolService.getRolPermisoUsuario().pipe(
      switchMap((todas) => {
        const asignacionesDelUsuario = todas.filter(
          (a) => a.userId === this.userId
        );

        if (asignacionesDelUsuario.length === 0) {
          return of([]); // no tenía nada asignado antes, seguimos directo
        }

        const eliminaciones$ = asignacionesDelUsuario.map((a) =>
          this.rolService.deleteRolUsuario(a.iD_Usuario_Rol_Permiso).pipe(
            catchError((err) => {
              console.error('Error al borrar asignación previa:', err);
              return of(null);
            })
          )
        );

        return forkJoin(eliminaciones$);
      }),
      switchMap(() => {
        // 🔹 3. Recién ahora insertamos los roles/permisos nuevos
        const creaciones$ = rolData.rolpermiso.map((permiso: any) => {
          const rolpermisoUsuario: RolPermisoUsuario = {
            userId: this.userId,
            iD_Rol_Permiso: permiso.iD_Rol_Permiso,
          };
          return this.rolService.createRolUsuario(rolpermisoUsuario).pipe(
            catchError((err) => {
              console.error('Error al insertar permiso:', err);
              return of(null);
            })
          );
        });

        return forkJoin(creaciones$);
      })
    ).subscribe({
      next: (respuestas) => {
        console.log('Roles reasignados correctamente:', respuestas);
        alert('✅ Cambios guardados con éxito.');
        this.router.navigate(['/dashboard/roles-permisos-usuario']);
      },
      error: (err) => {
        console.error('Error al guardar los roles:', err);
        alert('❌ Ocurrió un error al guardar los cambios.');
      },
    });
  }
}