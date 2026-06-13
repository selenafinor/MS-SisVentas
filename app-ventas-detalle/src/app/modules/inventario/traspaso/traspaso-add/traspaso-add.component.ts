import { Component, OnInit, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { TraspasoService } from '../service/traspaso.service';

@Component({
  selector: 'app-traspaso-add',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './traspaso-add.component.html',
  styleUrl: './traspaso-add.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TraspasoAddComponent implements OnInit {
  form!: FormGroup;
  articulosAlmacen: any[] = [];
  almacenes: any[] = [];
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private traspasoService: TraspasoService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.buildForm();
    this.loadData();
  }

  buildForm(): void {
    this.form = this.fb.group({
      fecha: [new Date().toISOString().split('T')[0], Validators.required],
      glosa: ['', [Validators.required, Validators.maxLength(200)]],
      estado: ['activo', Validators.required],
      id_AlmacenOrigen: [null, Validators.required],
      id_AlmacenDestino: [null, Validators.required],
      detalles: this.fb.array([]),
    });
  }

  loadData(): void {
    this.traspasoService.getAlmacenes().subscribe({
      next: (data) => {
        this.almacenes = data;
        this.cdr.markForCheck();
      },
    });
    this.traspasoService.getArticulosAlmacen().subscribe({
      next: (data) => {
        this.articulosAlmacen = data;
        this.cdr.markForCheck();
      },
    });
  }

  get detalles(): FormArray {
    return this.form.get('detalles') as FormArray;
  }

  newDetalle(): FormGroup {
    return this.fb.group({
      id_ArticuloAlmacen: [null, Validators.required],
      cantidad: [1, [Validators.required, Validators.min(1)]],
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
        text: 'Agrega al menos un artículo al traspaso.',
        icon: 'warning',
        background: '#111827',
        color: '#fff',
      });
      return;
    }

    const origenId = this.form.value.id_AlmacenOrigen;
    const destinoId = this.form.value.id_AlmacenDestino;
    if (origenId === destinoId) {
      Swal.fire({
        title: 'Almacenes iguales',
        text: 'El almacén de origen y destino no pueden ser el mismo.',
        icon: 'warning',
        background: '#111827',
        color: '#fff',
      });
      return;
    }

    this.isSubmitting = true;
    const { detalles, ...traspasoData } = this.form.value;

    this.traspasoService.addTraspaso(traspasoData).subscribe({
      next: (traspaso) => {
        if (!traspaso?.id) {
          this.handleError('No se pudo crear el traspaso.');
          return;
        }
        let completed = 0;
        detalles.forEach((d: any) => {
          this.traspasoService.addDetalleTraspaso({ ...d, id_Traspaso: traspaso.id }).subscribe({
            next: () => {
              completed++;
              if (completed === detalles.length) this.onSuccess();
            },
            error: () => this.handleError('Error al guardar un detalle.'),
          });
        });
      },
      error: () => this.handleError('Error al crear el traspaso.'),
    });
  }

  private onSuccess(): void {
    this.isSubmitting = false;
    Swal.fire({
      title: '¡Guardado!',
      text: 'Traspaso registrado correctamente.',
      icon: 'success',
      background: '#111827',
      color: '#fff',
    }).then(() => this.router.navigate(['/dashboard/traspaso']));
  }

  private handleError(msg: string): void {
    this.isSubmitting = false;
    this.cdr.markForCheck();
    Swal.fire({ title: 'Error', text: msg, icon: 'error', background: '#111827', color: '#fff' });
  }

  cancelar(): void {
    this.router.navigate(['/dashboard/traspaso']);
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