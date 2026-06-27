import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CatalogoProveedorService } from '../service/catalogo-proveedor.service';
import { ProveedorService } from '../service/proveedor.service';
import { ProductService } from '../../inventario/articulo/service/product.service';
import { CatalogoProveedor } from '../../../interfaces/catalogo-proveedor.interface';
import { Proveedor } from '../../../interfaces/proveedor.interface';
import { Articulo } from '../../../interfaces/articulo.interface';

type Modo = 'lista' | 'nuevo' | 'editar';

@Component({
  selector: 'app-catalogo',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './catalogo.component.html',
  styleUrl: './catalogo.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CatalogoComponent implements OnInit {

  modo: Modo = 'lista';

  // Datos
  catalogo: CatalogoProveedor[] = [];
  catalogoFiltrado: CatalogoProveedor[] = [];
  proveedores: Proveedor[] = [];
  articulos: Articulo[] = [];

  // Filtros
  filtroProveedorId: string = '';
  filtroArticulo: string = '';

  // Formulario nuevo
  formNuevo = {
    proveedorId: '',
    productoId: '',
    nombreProducto: '',
    precioUnitario: '',
    stockDisponible: ''
  };

  // Formulario editar
  itemSeleccionado: CatalogoProveedor | null = null;
  formEditar = {
    precioUnitario: '',
    stockDisponible: '',
    estado: 'activo'
  };

  guardando = signal<boolean>(false);
  mensaje: string = '';

  constructor(
    private catalogoService: CatalogoProveedorService,
    private proveedorService: ProveedorService,
    private productService: ProductService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.cargarDatos();
  }

  cargarDatos(): void {
    this.catalogoService.getAll().subscribe(data => {
      this.catalogo = data;
      this.aplicarFiltros();
      this.cdr.markForCheck();
    });
    this.proveedorService.getProveedorAll().subscribe(data => {
      this.proveedores = data.filter(p => p.estado === 'activo');
      this.cdr.markForCheck();
    });
    this.productService.getProductAll().subscribe(data => {
      this.articulos = data;
      this.cdr.markForCheck();
    });
  }

  aplicarFiltros(): void {
    this.catalogoFiltrado = this.catalogo.filter(c => {
      const matchProveedor = this.filtroProveedorId
        ? c.proveedorId === +this.filtroProveedorId : true;
      const matchArticulo = this.filtroArticulo
        ? c.nombreProducto?.toLowerCase().includes(this.filtroArticulo.toLowerCase()) : true;
      return matchProveedor && matchArticulo;
    });
    this.cdr.markForCheck();
  }

  onProductoSeleccionado(): void {
    const articulo = this.articulos.find(a => a.id === +this.formNuevo.productoId);
    this.formNuevo.nombreProducto = articulo?.nombre || '';
    this.cdr.markForCheck();
  }

  irANuevo(): void {
    this.modo = 'nuevo';
    this.mensaje = '';
    this.formNuevo = { proveedorId: '', productoId: '', nombreProducto: '', precioUnitario: '', stockDisponible: '' };
    this.cdr.markForCheck();
  }

  irALista(): void {
    this.modo = 'lista';
    this.mensaje = '';
    this.itemSeleccionado = null;
    this.cdr.markForCheck();
  }

  guardarNuevo(): void {
    if (!this.formNuevo.proveedorId || !this.formNuevo.productoId ||
        !this.formNuevo.precioUnitario || !this.formNuevo.stockDisponible) {
      this.mensaje = 'Complete todos los campos obligatorios.';
      this.cdr.markForCheck();
      return;
    }
    this.guardando.set(true);
    const item: CatalogoProveedor = {
      proveedorId:     +this.formNuevo.proveedorId,
      productoId:      +this.formNuevo.productoId,
      nombreProducto:  this.formNuevo.nombreProducto,
      precioUnitario:  +this.formNuevo.precioUnitario,
      stockDisponible: +this.formNuevo.stockDisponible,
      estado:          'activo'
    };
    this.catalogoService.create(item).subscribe({
      next: () => {
        this.guardando.set(false);
        this.cargarDatos();
        this.irALista();
      },
      error: () => {
        this.mensaje = 'Error al guardar el artículo.';
        this.guardando.set(false);
        this.cdr.markForCheck();
      }
    });
  }

  seleccionarEditar(item: CatalogoProveedor): void {
    this.itemSeleccionado = item;
    this.formEditar = {
      precioUnitario:  String(item.precioUnitario),
      stockDisponible: String(item.stockDisponible),
      estado:          item.estado || 'activo'
    };
    this.modo = 'editar';
    this.mensaje = '';
    this.cdr.markForCheck();
  }

  guardarEditar(): void {
    if (!this.itemSeleccionado) return;
    this.guardando.set(true);
    const item: CatalogoProveedor = {
      ...this.itemSeleccionado,
      precioUnitario:  +this.formEditar.precioUnitario,
      stockDisponible: +this.formEditar.stockDisponible,
      estado:          this.formEditar.estado
    };
    this.catalogoService.update(item).subscribe({
      next: () => {
        this.guardando.set(false);
        this.cargarDatos();
        this.irALista();
      },
      error: () => {
        this.mensaje = 'Error al actualizar.';
        this.guardando.set(false);
        this.cdr.markForCheck();
      }
    });
  }

  toggleEstado(item: CatalogoProveedor): void {
    if (!confirm(`¿${item.estado === 'activo' ? 'Deshabilitar' : 'Habilitar'} este artículo del catálogo?`)) return;
    const updated: CatalogoProveedor = {
      ...item,
      estado: item.estado === 'activo' ? 'inactivo' : 'activo'
    };
    this.catalogoService.update(updated).subscribe(() => {
      this.cargarDatos();
    });
  }

  getStockBadgeClass(stock: number): string {
    if (stock > 10) return 'bg-green-900 text-green-400';
    if (stock > 0)  return 'bg-yellow-900 text-yellow-400';
    return 'bg-red-900 text-red-400';
  }

  getNombreProveedor(proveedorId: number): string {
    return this.proveedores.find(p => p.id === proveedorId)?.nombre || '—';
  }
}