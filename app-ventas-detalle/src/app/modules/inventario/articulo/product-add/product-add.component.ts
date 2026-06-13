import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Input,
  OnInit,
} from '@angular/core';
import { Category } from '../../../../interfaces/category.interface';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ProductService } from '../service/product.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';
import { CategoryService } from '../../category/service/category.service';

@Component({
  selector: 'app-product-add',
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  template: `
    <div class="max-w-lg mx-auto mt-10 p-8 bg-gradient-to-br from-gray-900 via-gray-800 to-blue-900 rounded-2xl shadow-[0_4px_20px_rgba(0,0,0,0.5)] border border-blue-500/30">
      <h2 class="text-3xl font-extrabold text-center mb-6 bg-gradient-to-r from-blue-400 via-blue-600 to-cyan-500 bg-clip-text text-transparent">
        🚀 Crear Producto
      </h2>

      <form [formGroup]="productForm" (ngSubmit)="createProduct()" class="space-y-5">

        <div>
          <label class="block text-sm font-semibold text-gray-300">📌 Nombre</label>
          <input type="text" formControlName="nombre" placeholder="Ej: Cable USB Tipo C"
            class="w-full px-4 py-3 mt-2 bg-gray-800 text-white rounded-lg border border-gray-700 focus:outline-none focus:ring-2 focus:ring-cyan-500 transition" />
          <div *ngIf="productForm.get('nombre')?.invalid && productForm.get('nombre')?.touched" class="mt-1 text-red-400 text-sm">
            ⚠️ El nombre es requerido (máx. 30 caracteres).
          </div>
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-300">💰 Precio</label>
          <input type="number" formControlName="precio" placeholder="Ej: 15.50"
            class="w-full px-4 py-3 mt-2 bg-gray-800 text-white rounded-lg border border-gray-700 focus:outline-none focus:ring-2 focus:ring-cyan-500 transition" />
          <div *ngIf="productForm.get('precio')?.invalid && productForm.get('precio')?.touched" class="mt-1 text-red-400 text-sm">
            ⚠️ El precio debe ser mayor a 0.
          </div>
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-300">📦 Categoría</label>
          <select formControlName="id_categoria"
            class="w-full px-4 py-3 mt-2 bg-gray-800 text-white rounded-lg border border-gray-700 focus:outline-none focus:ring-2 focus:ring-cyan-500 transition">
            <option value="" disabled selected>Seleccione una categoría</option>
            <option *ngFor="let cat of categories" [value]="cat.id">{{ cat.nombre }}</option>
          </select>
          <div *ngIf="productForm.get('id_categoria')?.invalid && productForm.get('id_categoria')?.touched" class="mt-1 text-red-400 text-sm">
            ⚠️ Seleccione una categoría.
          </div>
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-300">🏷️ Marca</label>
          <select formControlName="id_Marca"
            class="w-full px-4 py-3 mt-2 bg-gray-800 text-white rounded-lg border border-gray-700 focus:outline-none focus:ring-2 focus:ring-cyan-500 transition">
            <option value="" disabled selected>Seleccione una marca</option>
            <option *ngFor="let marca of marcas" [value]="marca.id">{{ marca.nombre }}</option>
          </select>
          <div *ngIf="productForm.get('id_Marca')?.invalid && productForm.get('id_Marca')?.touched" class="mt-1 text-red-400 text-sm">
            ⚠️ Seleccione una marca.
          </div>
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-300">📏 Unidad de Medida</label>
          <select formControlName="id_UnidadMedida"
            class="w-full px-4 py-3 mt-2 bg-gray-800 text-white rounded-lg border border-gray-700 focus:outline-none focus:ring-2 focus:ring-cyan-500 transition">
            <option value="" disabled selected>Seleccione unidad</option>
            <option *ngFor="let unidad of unidades" [value]="unidad.id">{{ unidad.nombre }}</option>
          </select>
          <div *ngIf="productForm.get('id_UnidadMedida')?.invalid && productForm.get('id_UnidadMedida')?.touched" class="mt-1 text-red-400 text-sm">
            ⚠️ Seleccione una unidad de medida.
          </div>
        </div>

        <button type="submit"
          class="w-full py-3 bg-gradient-to-r from-blue-600 via-blue-500 to-cyan-500 text-white font-bold rounded-lg hover:scale-105 transition-all duration-300 disabled:opacity-50 disabled:cursor-not-allowed"
          [disabled]="productForm.invalid">
          ➕ Agregar Producto
        </button>

      </form>
    </div>
  `,
  styleUrl: './product-add.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProductAddComponent implements OnInit {
  @Input() category: Category[] = [];
  categories: Category[] = [];
  marcas: { id: number; nombre: string }[] = [
    { id: 1, nombre: 'Samsung' },
    { id: 2, nombre: 'LG' },
    { id: 3, nombre: 'Sony' },
    { id: 4, nombre: 'Panasonic' },
    { id: 5, nombre: 'Philips' },
    { id: 6, nombre: 'Bosch' },
    { id: 7, nombre: 'TP-Link' },
    { id: 8, nombre: 'Genérico' },
  ];
  unidades: { id: number; nombre: string }[] = [
    { id: 1, nombre: 'Unidad' },
    { id: 2, nombre: 'Par' },
    { id: 3, nombre: 'Caja' },
    { id: 4, nombre: 'Metro' },
  ];

  productForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private router: Router,
    private categoryService: CategoryService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.productForm = this.fb.group({
      nombre: ['', [Validators.required, Validators.maxLength(30)]],
      precio: [0, [Validators.required, Validators.min(0.01)]],
      estado: ['activo'],
      id_categoria: [null, [Validators.required]],
      id_Marca: [null, [Validators.required]],
      id_UnidadMedida: [null, [Validators.required]],
    });
    this.loadCategories();
  }

  loadCategories(): void {
    this.categoryService.getCategoryAll().subscribe(
      (data) => {
        this.categories = data;
        this.cdr.markForCheck();
      },
      (error) => {
        console.error(error);
        this.cdr.markForCheck();
      }
    );
  }

  createProduct(): void {
    if (this.productForm.valid) {
      const product = this.productForm.value;
      this.productService.createProduct(product).subscribe({
        next: (response) => {
          Swal.fire({
            icon: 'success',
            title: 'Producto creado',
            text: 'El producto se ha creado exitosamente.',
            confirmButtonText: 'OK',
            background: '#111827',
            color: '#fff',
          }).then(() => {
            this.router.navigate(['/dashboard/articulo/list']);
          });
        },
        error: (err) => {
          console.error(err);
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Hubo un problema al crear el producto.',
            confirmButtonText: 'OK',
            background: '#111827',
            color: '#fff',
          });
        },
      });
    }
  }
}