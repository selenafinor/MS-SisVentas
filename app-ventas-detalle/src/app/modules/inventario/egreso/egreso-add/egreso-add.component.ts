import { Component, OnInit, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { EgresoService } from '../service/egreso.service';

@Component({
  selector: 'app-egreso-add',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './egreso-add.component.html',
  styleUrl: './egreso-add.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EgresoAddComponent implements OnInit {
  form!: FormGroup;
  articulosAlmacen: any[] = [];
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private egresoService: EgresoService,
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
    this.egresoService.getArticulosAlmacen().subscribe({
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
        text: 'Agrega al menos un artículo al egreso.',
        icon: 'warning',
        background: '#111827',
        color: '#fff',
      });
      return;
    }

    this.isSubmitting = true;
    const { detalles, ...egresoData } = this.form.value;

    this.egresoService.addEgreso(egresoData).subscribe({
      next: (egreso) => {
        if (!egreso?.id) {
          this.handleError('No se pudo crear el egreso.');
          return;
        }
        let completed = 0;
        detalles.forEach((d: any) => {
          this.egresoService.addDetalleEgreso({ ...d, id_Egreso: egreso.id }).subscribe({
            next: () => {
              completed++;
              if (completed === detalles.length) this.onSuccess();
            },
            error: () => this.handleError('Error al guardar un detalle.'),
          });
        });
      },
      error: () => this.handleError('Error al crear el egreso.'),
    });
  }

  private onSuccess(): void {
    this.isSubmitting = false;
    Swal.fire({
      title: '¡Guardado!',
      text: 'Egreso registrado correctamente.',
      icon: 'success',
      background: '#111827',
      color: '#fff',
    }).then(() => this.router.navigate(['/dashboard/egreso']));
  }

  private handleError(msg: string): void {
    this.isSubmitting = false;
    this.cdr.markForCheck();
    Swal.fire({ title: 'Error', text: msg, icon: 'error', background: '#111827', color: '#fff' });
  }

  cancelar(): void {
    this.router.navigate(['/dashboard/egreso']);
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