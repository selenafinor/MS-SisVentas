import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { OrdenCompraService } from '../../service/orden-compra.service';
import { CatalogoProveedorService } from '../../service/catalogo-proveedor.service';
import { ProductService } from '../../../inventario/articulo/service/product.service';
import { DetalleOrdenCompra } from '../../../../interfaces/detalle-orden-compra.interface';
import { CatalogoProveedor } from '../../../../interfaces/catalogo-proveedor.interface';
import { Product } from '../../../../interfaces/poduct.interface';


@Component({
  selector: 'app-orden-compra-add',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './orden-compra-add.component.html',
  styleUrl: './orden-compra-add.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OrdenCompraAddComponent implements OnInit {

  // Búsqueda de artículo
  busquedaArticulo: string = '';
  articulosFiltrados: Product[] = [];
  todosArticulos: Product[] = [];
  articuloSeleccionado: Product | null = null;
  // Proveedores disponibles para el artículo seleccionado
  proveedoresDisponibles: CatalogoProveedor[] = [];
  cargandoProveedores = false;

  // Carrito
  carrito = signal<DetalleOrdenCompra[]>([]);
  total = signal<number>(0);

  // Otros campos
  glosa: string = '';
  cargando = signal<boolean>(false);

  constructor(
    private ordenService: OrdenCompraService,
    private catalogoService: CatalogoProveedorService,
    private productService: ProductService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.productService.getProductAll().subscribe(data => {
      this.todosArticulos = data;
      this.cdr.markForCheck();
    });
  }

  filtrarArticulos(): void {
    const q = this.busquedaArticulo.toLowerCase().trim();
    if (!q) {
      this.articulosFiltrados = [];
    } else {
      this.articulosFiltrados = this.todosArticulos.filter(a =>
        a.nombre?.toLowerCase().includes(q)
      ).slice(0, 8);
    }
    this.cdr.markForCheck();
  }

  seleccionarArticulo(articulo: Product): void {
    this.articuloSeleccionado = articulo;
    this.busquedaArticulo = articulo.nombre || '';
    this.articulosFiltrados = [];
    this.proveedoresDisponibles = [];
    this.cargandoProveedores = true;
    this.cdr.markForCheck();

    this.catalogoService.getByProducto(articulo.id!).subscribe(data => {
      this.proveedoresDisponibles = data;
      this.cargandoProveedores = false;
      this.cdr.markForCheck();
    });
  }

  cambiarArticulo(): void {
    this.articuloSeleccionado = null;
    this.busquedaArticulo = '';
    this.proveedoresDisponibles = [];
    this.articulosFiltrados = [];
    this.cdr.markForCheck();
  }

  seleccionarProveedor(catalogo: CatalogoProveedor): void {
    const existente = this.carrito().find(
      i => i.productoId === catalogo.productoId
    );
    if (existente) {
      alert('Este artículo ya está en el carrito. Quítalo primero si quieres cambiarlo.');
      return;
    }

    const item: DetalleOrdenCompra = {
  productoId:     catalogo.productoId,
  nombreProducto: this.todosArticulos.find(a => a.id === catalogo.productoId)?.nombre || '',
  cantidad:       1,
  precioUni:      catalogo.precioUnitario,
  subTotal:       catalogo.precioUnitario
};
    this.carrito.update(c => [...c, item]);
    this.calcularTotal();
    this.cambiarArticulo();
    this.cdr.markForCheck();
  }

  actualizarCantidad(index: number, cantidad: number): void {
    if (cantidad < 1) return;
    const item = this.carrito()[index];
    item.cantidad = cantidad;
    item.subTotal = cantidad * (item.precioUni || 0);
    this.carrito.update(c => [...c]);
    this.calcularTotal();
    this.cdr.markForCheck();
  }

  quitarDelCarrito(index: number): void {
    this.carrito.update(c => c.filter((_, i) => i !== index));
    this.calcularTotal();
    this.cdr.markForCheck();
  }

  calcularTotal(): void {
    const t = this.carrito().reduce((sum, i) => sum + (i.subTotal || 0), 0);
    this.total.set(t);
  }

  async crearOrden(): Promise<void> {
    if (this.carrito().length === 0) { alert('Agregue al menos un artículo'); return; }

    this.cargando.set(true);
    const user = JSON.parse(sessionStorage.getItem('user') || '{}');

    // Tomamos el proveedor del primer item del carrito como proveedor principal
    // (en una orden real se puede tener un proveedor por artículo — aquí simplificamos)
    const primerItem = this.carrito()[0];
    const catalogoPrimerItem = await this.catalogoService
      .getByProducto(primerItem.productoId!).toPromise();
    const proveedorId = catalogoPrimerItem?.[0]?.proveedorId || null;

    try {
      const orden = await this.ordenService.create({
        proveedorId: proveedorId ?? undefined,
        glosa: this.glosa,
        usuarioId: user.userId || 1
      }).toPromise();

      if (!orden?.id) throw new Error('No se pudo crear la orden');

      for (const item of this.carrito()) {
        await this.ordenService.agregarDetalle(orden.id, item).toPromise();
      }

      alert('Orden de compra creada exitosamente');
      this.router.navigate(['/dashboard/compra/orden-compra/list']);
    } catch (error) {
      console.error('Error al crear orden:', error);
      alert('Error al crear la orden. Intente nuevamente.');
    } finally {
      this.cargando.set(false);
      this.cdr.markForCheck();
    }
  }
}