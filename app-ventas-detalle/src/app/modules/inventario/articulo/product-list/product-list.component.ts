import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input } from '@angular/core';
import { Product } from '../../../../interfaces/poduct.interface';
import { ProductService } from '../service/product.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';
import { CategoryService } from '../../category/service/category.service';

@Component({
  selector: 'app-product-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProductListComponent {
  @Input() public products: Product[] = [];
  message: string = '';
  productoSeleccionado: Product | null = null;
  stockPorAlmacen: any[] = [];
  editandoLimites: any | null = null;
  stockMinEdit: number = 0;
  stockMaxEdit: number = 0;

  // Ajuste de stock
  ajustandoProducto: Product | null = null;
  tipoAjuste: string = 'agregar';
  cantidadAjuste: number = 1;
  motivoAjuste: string = '';
  almacenesAjuste: any[] = [];
  almacenAjusteSeleccionado: any | null = null;

  // Editar artículo
  editandoProducto: Product | null = null;
  editNombre: string = '';
  editDescripcion: string = '';
  editPrecio: number = 0;
  editEstado: string = 'activo';
  editIdCategoria: number = 0;
  editIdMarca: number = 0;
  editIdUnidadMedida: number = 0;
  editFotoFile: File | null = null;
  editFotoPreview: string | null = null;
  categories: any[] = [];
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

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  verStock(producto: Product): void {
    if (this.productoSeleccionado?.id === producto.id) {
      this.productoSeleccionado = null;
      this.stockPorAlmacen = [];
    } else {
      this.productoSeleccionado = producto;
      this.cargarStockPorAlmacen(producto.id!);
    }
    this.editandoLimites = null;
    this.cdr.markForCheck();
  }

  cargarStockPorAlmacen(articuloId: number): void {
    this.productService.getStockPorArticulo(articuloId).subscribe({
      next: (data) => {
        this.stockPorAlmacen = data.filter(aa => aa.id_Articulo === articuloId);
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });
  }

  abrirEditarLimites(aa: any): void {
    this.editandoLimites = aa;
    this.stockMinEdit = aa.stockMin ?? 0;
    this.stockMaxEdit = aa.stockMax ?? 0;
    this.cdr.markForCheck();
  }

  cerrarEditarLimites(): void {
    this.editandoLimites = null;
    this.cdr.markForCheck();
  }

  guardarLimites(): void {
    if (!this.editandoLimites) return;
    if (this.stockMaxEdit < this.stockMinEdit) {
      Swal.fire({ title: 'Error', text: 'El stock máximo no puede ser menor al mínimo.', icon: 'warning', background: '#111827', color: '#fff' });
      return;
    }
    this.productService.updateLimites(
      this.editandoLimites.id,
      this.editandoLimites.stock ?? 0,
      this.stockMinEdit,
      this.stockMaxEdit,
      this.editandoLimites.id_Articulo,
      this.editandoLimites.id_Almacen
    ).subscribe({
      next: () => {
        this.editandoLimites.stockMin = this.stockMinEdit;
        this.editandoLimites.stockMax = this.stockMaxEdit;
        this.editandoLimites = null;
        Swal.fire({ title: '¡Guardado!', text: 'Límites actualizados.', icon: 'success', background: '#111827', color: '#fff' });
        if (this.productoSeleccionado) this.cargarStockPorAlmacen(this.productoSeleccionado.id!);
      },
      error: (err) => {
        console.error(err);
        Swal.fire({ title: 'Error', text: 'No se pudo actualizar.', icon: 'error', background: '#111827', color: '#fff' });
      }
    });
  }

  abrirAjuste(producto: Product): void {
    this.ajustandoProducto = producto;
    this.tipoAjuste = 'agregar';
    this.cantidadAjuste = 1;
    this.motivoAjuste = '';
    this.almacenAjusteSeleccionado = null;
    this.productService.getStockPorArticulo(producto.id!).subscribe({
      next: (data) => {
        this.almacenesAjuste = data.filter(aa => aa.id_Articulo === producto.id);
        this.cdr.markForCheck();
      }
    });
    this.cdr.markForCheck();
  }

  cerrarAjuste(): void {
    this.ajustandoProducto = null;
    this.almacenAjusteSeleccionado = null;
    this.cdr.markForCheck();
  }

  aplicarAjuste(): void {
    if (!this.ajustandoProducto) return;
    if (!this.almacenAjusteSeleccionado) {
      Swal.fire({ title: 'Error', text: 'Selecciona un almacén.', icon: 'warning', background: '#111827', color: '#fff' });
      return;
    }
    if (this.cantidadAjuste <= 0) {
      Swal.fire({ title: 'Error', text: 'La cantidad debe ser mayor a 0.', icon: 'warning', background: '#111827', color: '#fff' });
      return;
    }

    const stockActual = this.almacenAjusteSeleccionado.stock ?? 0;
    let nuevoStock = this.tipoAjuste === 'agregar'
      ? stockActual + this.cantidadAjuste
      : stockActual - this.cantidadAjuste;

    if (nuevoStock < 0) {
      Swal.fire({ title: 'Error', text: 'El stock no puede ser negativo.', icon: 'warning', background: '#111827', color: '#fff' });
      return;
    }

    this.productService.updateLimites(
      this.almacenAjusteSeleccionado.id,
      nuevoStock,
      this.almacenAjusteSeleccionado.stockMin ?? 0,
      this.almacenAjusteSeleccionado.stockMax ?? 0,
      this.almacenAjusteSeleccionado.id_Articulo,
      this.almacenAjusteSeleccionado.id_Almacen
    ).subscribe({
      next: () => {
        this.ajustandoProducto = null;
        this.almacenAjusteSeleccionado = null;
        Swal.fire({ title: '¡Ajustado!', text: 'Stock actualizado correctamente.', icon: 'success', background: '#111827', color: '#fff' });
        this.reloadProducts();
      },
      error: (err) => {
        console.error(err);
        Swal.fire({ title: 'Error', text: 'No se pudo ajustar el stock.', icon: 'error', background: '#111827', color: '#fff' });
      }
    });
  }

  abrirEditar(producto: Product): void {
    this.editandoProducto = producto;
    this.editNombre = producto.nombre ?? '';
    this.editDescripcion = producto.descripcion ?? '';
    this.editPrecio = producto.precio;
    this.editEstado = producto.estado ?? 'activo';
    this.editIdCategoria = producto.id_Categoria ?? producto.id_categoria ?? 0;
    this.editIdMarca = producto.id_Marca ?? 0;
    this.editIdUnidadMedida = producto.id_UnidadMedida ?? 0;
    this.editFotoFile = null;
    this.editFotoPreview = producto.foto ?? null;
    this.categoryService.getCategoryAll().subscribe({
      next: (data) => {
        this.categories = data;
        this.cdr.markForCheck();
      }
    });
    this.cdr.markForCheck();
  }

  cerrarEditar(): void {
    this.editandoProducto = null;
    this.editFotoFile = null;
    this.editFotoPreview = null;
    this.cdr.markForCheck();
  }

  onFotoSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.editFotoFile = file;
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.editFotoPreview = e.target.result;
        this.cdr.markForCheck();
      };
      reader.readAsDataURL(file);
    }
  }

  guardarEdicion(): void {
    if (!this.editandoProducto) return;

    const body = {
      id: this.editandoProducto.id,
      nombre: this.editNombre,
      descripcion: this.editDescripcion,
      precio: this.editPrecio,
      estado: this.editEstado,
      id_Categoria: this.editIdCategoria,
      id_Marca: this.editIdMarca,
      id_UnidadMedida: this.editIdUnidadMedida,
    };

    this.productService.updateArticulo(this.editandoProducto.id!, body).subscribe({
      next: () => {
        if (this.editFotoFile) {
          this.productService.uploadFoto(this.editandoProducto!.id!, this.editFotoFile).subscribe({
            next: () => {
              this.finalizarEdicion();
            },
            error: () => this.finalizarEdicion()
          });
        } else {
          this.finalizarEdicion();
        }
      },
      error: (err) => {
        console.error(err);
        Swal.fire({ title: 'Error', text: 'No se pudo actualizar el artículo.', icon: 'error', background: '#111827', color: '#fff' });
      }
    });
  }

  private finalizarEdicion(): void {
    this.editandoProducto = null;
    this.editFotoFile = null;
    Swal.fire({ title: '¡Guardado!', text: 'Artículo actualizado correctamente.', icon: 'success', background: '#111827', color: '#fff' });
    this.reloadProducts();
  }

  reloadProducts(): void {
    this.productService.getProductAll().subscribe({
      next: (products) => {
        this.products = products;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });
  }

  agregarProducto(): void {
    this.router.navigate(['/dashboard/articulo/add']);
  }
}