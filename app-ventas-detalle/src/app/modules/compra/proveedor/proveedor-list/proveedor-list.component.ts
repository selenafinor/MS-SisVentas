import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ProveedorService } from '../../service/proveedor.service';
import { Proveedor } from '../../../../interfaces/proveedor.interface';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CatalogoProveedorService } from '../../service/catalogo-proveedor.service';
import { CatalogoProveedor } from '../../../../interfaces/catalogo-proveedor.interface';
import { ProductService } from '../../../inventario/articulo/service/product.service';
import { Product } from '../../../../interfaces/poduct.interface';

@Component({
  selector: 'app-proveedor-list',
  imports: [CommonModule, RouterModule],
  templateUrl: './proveedor-list.component.html',
  styleUrl: './proveedor-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProveedorListComponent implements OnInit {
  proveedores: Proveedor[] = [];
  errorMessage: string = '';
  proveedorExpandido: number | null = null;
  catalogoActual: CatalogoProveedor[] = [];
  cargandoCatalogo: boolean = false;
  articulos: Product[] = [];
  constructor(
    private proveedorService: ProveedorService,
    private cdr: ChangeDetectorRef,
    private catalogoService: CatalogoProveedorService,
    private productService: ProductService,

    
  ) {}

  ngOnInit(): void {
  this.loadProveedores();
  this.productService.getProductAll().subscribe(data => {
    this.articulos = data;
    this.cdr.markForCheck();
  });
}

  loadProveedores(): void {
    this.proveedorService.getProveedorAll().subscribe({
      next: (data) => {
        if (Array.isArray(data) && data.length > 0) {
          this.proveedores = data;
          this.errorMessage = '';
        } else {
          this.proveedores = [];
          this.errorMessage = 'No se encontraron proveedores disponibles.';
        }
        this.cdr.markForCheck();
      },
      error: () => {
        this.errorMessage = 'Error al cargar los proveedores.';
        this.proveedores = [];
        this.cdr.markForCheck();
      }
    });
  }

  toggleEstado(id: number): void {
    this.proveedorService.toggleEstado(id).subscribe(() => {
      this.loadProveedores();
    });
  }
  toggleCatalogo(id: number): void {
    if (this.proveedorExpandido === id) {
      this.proveedorExpandido = null;
      this.catalogoActual = [];
      this.cdr.markForCheck();
      return;
    }
    this.proveedorExpandido = id;
    this.cargandoCatalogo = true;
    this.catalogoService.getByProveedor(id).subscribe(data => {
      this.catalogoActual = data;
      this.cargandoCatalogo = false;
      this.cdr.markForCheck();
    });
  }
  getNombreArticulo(productoId: number | undefined): string {
  if (!productoId) return '—';
  return this.articulos.find(a => a.id === productoId)?.nombre || '—';
}
}