import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PermisoService } from '../permiso.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-permiso-add',
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './permiso-add.component.html',
  styleUrl: './permiso-add.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PermisoAddComponent implements OnInit {
  permisoForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private permisoService: PermisoService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.permisoForm = this.fb.group({
      nombre_Permiso: ['', [Validators.required, Validators.maxLength(100)]],
      descripcion: [''],
    });
  }

  crearPermiso(): void {
    if (this.permisoForm.valid) {
      this.permisoService.createPermiso(this.permisoForm.value).subscribe({
        next: () => {
          Swal.fire('¡Creado!', 'Permiso creado correctamente.', 'success')
            .then(() => this.router.navigate(['/dashboard/permisos']));
        },
        error: (err) => console.error(err),
      });
    }
  }

  volver(): void {
    this.router.navigate(['/dashboard/permisos']);
  }
}