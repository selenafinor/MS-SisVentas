import { Component, OnInit, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { IngresoService } from '../service/ingreso.service';

@Component({
  selector: 'app-ingreso-add',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './ingreso-add.component.html',
  styleUrl: './ingreso-add.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class IngresoAddComponent implements OnInit {
  form!: FormGroup;
  articulosAlmacen: any[] = [];
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private ingresoService: IngresoService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.buildForm();
    this.loadArticulosAlmacen();
  }

  buildForm(): void {
    this.form = this.fb.group({
      fecha: [new Date().toISOString().split('T')[0], Validators.required],
      glosa: ['', [Validators.required, Validators.maxLength(200)]],
      motivo: ['', [Validators.required, Validators.maxLength(200)]],
      estado: ['activo', Validators.required],
      detalles: this.fb.array([]),
    });
  }

  loadArticulosAlmacen(): void {
    this.ingresoService.getArticulosAlmacen().subscribe({
      next: (data) => {
        this.articulosAlmacen = data;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });
  }

  get detalles(): FormArray {
    return this.form.get('detalles') as FormArray;
  }

  newDetalle(): FormGroup {
    return this.fb.group({
      id_ArticuloAlmacen: [null, Validators.required],
      cantidad: [1, [Validators.required, Validators.min(1)]],
      precioCompra: [0, [Validators.required, Validators.min(0)]],
      observacion: [''],
    });
  }

  agregarDetalle(): void {
    this.detalles.push(this.newDetalle());
  }

  quitarDetalle(index: number): void {
    this.detalles.removeAt(index);
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    if (this.detalles.length === 0) {
      Swal.fire({
        title: 'Sin detalles',
        text: 'Agrega al menos un artículo al ingreso.',
        icon: 'warning',
        background: '#111827',
        color: '#fff',
      });
      return;
    }

    this.isSubmitting = true;
    const { detalles, ...ingresoData } = this.form.value;

    this.ingresoService.addIngreso(ingresoData).subscribe({
      next: (ingreso) => {
        if (!ingreso?.id) {
          this.handleError('No se pudo crear el ingreso.');
          return;
        }
        let completed = 0;
        detalles.forEach((d: any) => {
          this.ingresoService.addDetalleIngreso({ ...d, id_Ingreso: ingreso.id }).subscribe({
            next: () => {
              completed++;
              if (completed === detalles.length) this.onSuccess();
            },
            error: () => this.handleError('Error al guardar un detalle.'),
          });
        });
      },
      error: () => this.handleError('Error al crear el ingreso.'),
    });
  }

  private onSuccess(): void {
    this.isSubmitting = false;
    Swal.fire({
      title: '¡Guardado!',
      text: 'Ingreso registrado correctamente.',
      icon: 'success',
      background: '#111827',
      color: '#fff',
    }).then(() => this.router.navigate(['/dashboard/ingreso']));
  }

  private handleError(msg: string): void {
    this.isSubmitting = false;
    this.cdr.markForCheck();
    Swal.fire({ title: 'Error', text: msg, icon: 'error', background: '#111827', color: '#fff' });
  }

  cancelar(): void {
    this.router.navigate(['/dashboard/ingreso']);
  }

  isInvalid(field: string): boolean {
    const ctrl = this.form.get(field);
    return !!(ctrl?.invalid && ctrl?.touched);
  }

  isDetalleInvalid(index: number, field: string): boolean {
    const ctrl = this.detalles.at(index).get(field);
    return !!(ctrl?.invalid && ctrl?.touched);
  }
}