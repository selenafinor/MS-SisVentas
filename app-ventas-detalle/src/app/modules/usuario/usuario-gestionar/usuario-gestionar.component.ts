import { CommonModule, DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UsuarioService } from '../usuario.service';
import { RolService } from '../../rol/rol.service';
import { Rol } from '../../../interfaces/rol.interface';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-usuario-gestionar',
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './usuario-gestionar.component.html',
  styleUrl: './usuario-gestionar.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UsuarioGestionarComponent implements OnInit {
  usuario: any = null;
  roles: Rol[] = [];
  rolSeleccionado: number | null = null;
  tabActiva: string = 'rol';
  editForm!: FormGroup;
  userId!: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private usuarioService: UsuarioService,
    private rolService: RolService,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.userId = Number(this.route.snapshot.paramMap.get('id'));

    this.editForm = this.fb.group({
      fullname: ['', Validators.required],
      username: ['', Validators.required],
      correo: ['', Validators.email],
      telefono: [''],
      estado: ['activo'],
    });

    this.usuarioService.getUsuarioById(this.userId).subscribe({
      next: (data) => {
        this.usuario = data;
        this.editForm.patchValue({
          fullname: data.fullname,
          username: data.username,
          correo: data.correo,
          telefono: data.telefono,
          estado: data.estado || 'activo',
        });
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });

    this.rolService.getRoles().subscribe({
      next: (roles) => {
        this.roles = roles;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });
  }

  asignarRol(): void {
    if (!this.rolSeleccionado) return;

    const rolPermisos = this.roles.find(r => r.iD_Rol === Number(this.rolSeleccionado))?.rolPermisos;

    if (!rolPermisos || rolPermisos.length === 0) {
      Swal.fire('Error', 'El rol no tiene permisos asignados.', 'warning');
      return;
    }

    // 🔹 1. Antes de asignar el rol nuevo, borramos las asignaciones
    // (RolPermisoUsuario) que el usuario ya tenía. Sin este paso, el
    // rol nuevo se suma al viejo en vez de reemplazarlo.
    this.rolService.getRolPermisoUsuario().toPromise().then((todas) => {
      const asignacionesDelUsuario = (todas || []).filter(
        (a) => a.userId === this.userId
      );

      const eliminaciones = asignacionesDelUsuario.map((a) =>
        this.rolService.deleteRolUsuario(a.iD_Usuario_Rol_Permiso).toPromise()
          .catch((err) => console.error('Error al borrar asignación previa:', err))
      );

      return Promise.all(eliminaciones);
    }).then(() => {
      // 🔹 2. Recién ahora creamos las asignaciones del rol nuevo
      const asignaciones = rolPermisos.map(rp =>
        this.rolService.createRolUsuario({
          userId: this.userId,
          iD_Rol_Permiso: rp.iD_Rol_Permiso!
        }).toPromise()
      );
      return Promise.all(asignaciones);
    }).then(() => {
      Swal.fire('¡Asignado!', 'Rol asignado correctamente.', 'success');
      this.cdr.markForCheck();
    }).catch(err => {
      console.error(err);
      Swal.fire('Error', 'Ocurrió un error al asignar el rol.', 'error');
    });
  }

 guardarDatos(): void {
  if (this.editForm.valid) {
    const datos = {
      userId: this.userId,
      fullname: this.editForm.value.fullname,
      username: this.editForm.value.username,
      correo: this.editForm.value.correo,
      telefono: this.editForm.value.telefono,
      estado: this.usuario?.estado || 'activo',
      password: this.usuario?.password || ''
    };
    this.usuarioService.updateUsuario(this.userId, datos).subscribe({
      next: () => {
        Swal.fire('¡Actualizado!', 'Datos actualizados correctamente.', 'success');
      },
      error: (err) => console.error(err),
    });
  }
}

  volver(): void {
    this.router.navigate(['/dashboard/user']);
  }
}