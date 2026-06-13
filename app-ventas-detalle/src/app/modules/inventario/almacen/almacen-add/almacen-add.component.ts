import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AlmacenService } from '../service/almacen.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-almacen-add',
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './almacen-add.component.html',
  styleUrl: './almacen-add.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AlmacenAddComponent implements OnInit {
  almacenForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private almacenService: AlmacenService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.almacenForm = this.fb.group({
      nombre: ['', [Validators.required, Validators.maxLength(100)]],
      descripcion: [''],
      direccion: [''],
      cantidadMax: [0, [Validators.required, Validators.min(0)]],
      estado: ['activo'],
    });
  }

  createAlmacen(): void {
    if (this.almacenForm.valid) {
      const almacen = this.almacenForm.value;
      this.almacenService.addAlmacen(almacen).subscribe({
        next: () => {
          Swal.fire({
            icon: 'success',
            title: 'Almacén creado',
            text: 'El almacén se ha creado exitosamente.',
            confirmButtonText: 'OK'
          }).then(() => {
            this.router.navigate(['/dashboard/almacen']);
          });
        },
        error: (err) => {
          console.error(err);
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Hubo un problema al crear el almacén.',
            confirmButtonText: 'OK'
          });
        }
      });
    }
  }

  cancelar(): void {
    this.router.navigate(['/dashboard/almacen']);
  }
}