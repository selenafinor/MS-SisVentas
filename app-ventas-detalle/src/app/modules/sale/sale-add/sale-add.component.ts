import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Sale } from '../../../interfaces/sale.interface';
import { SaleDetail } from '../../../interfaces/sale-detail.inteface';
import { Customer } from '../../../interfaces/customer.interface';
import { SaleService } from '../service/sale.service';
import { CustomerService } from '../../customer/service/customer.service';
import { ProductoAlmacenService } from '../../inventario/articulo/service/productoAlmacen.service';
import { ProductoAlmacen } from '../../../interfaces/producto-almacen,interface,';

@Component({
  selector: 'app-sale-add',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './sale-add.component.html',
  styleUrl: './sale-add.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SaleAddComponent implements OnInit {

  // Búsqueda de cliente
  terminoBusqueda: string = '';
  clientesEncontrados = signal<Customer[]>([]);
  clienteSeleccionado = signal<Customer | null>(null);

  // Productos
  productos = signal<ProductoAlmacen[]>([]);
  productosFiltrados = signal<ProductoAlmacen[]>([]);
  busquedaProducto: string = '';

  // Carrito
  carrito = signal<SaleDetail[]>([]);
  total = signal<number>(0);

  // Venta creada
  ventaCreada = signal<Sale | null>(null);

  // Opciones
  tipoPago: string = 'contado';
  glosa: string = '';

  cargando = signal<boolean>(false);
  mensaje = signal<string>('');

  // QR
  mostrarModalQr = signal<boolean>(false);
  qrUrl = signal<string | null>(null);
  urlPasarela = signal<string | null>(null);
  private pollingInterval: any = null;
  private ventaIdQr: number = 0;

  private get headers(): HttpHeaders {
    const token = sessionStorage.getItem('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token ?? ''}`,
      'Content-Type': 'application/json'
    });
  }

  constructor(
    private saleService: SaleService,
    private customerService: CustomerService,
    private productoAlmacenService: ProductoAlmacenService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.cargarProductos();
  }

  cargarProductos(): void {
    this.productoAlmacenService.getProductoAlmacenAll().subscribe(data => {
      this.productos.set(data);
      this.productosFiltrados.set(data);
      this.cdr.markForCheck();
    });
  }

  buscarCliente(): void {
    if (!this.terminoBusqueda.trim()) return;
    this.customerService.buscarClientes(this.terminoBusqueda).subscribe(data => {
      this.clientesEncontrados.set(data);
      this.cdr.markForCheck();
    });
  }

  seleccionarCliente(cliente: Customer): void {
    this.clienteSeleccionado.set(cliente);
    this.clientesEncontrados.set([]);
    this.terminoBusqueda = '';
    this.cdr.markForCheck();
  }

  limpiarCliente(): void {
    this.clienteSeleccionado.set(null);
    this.terminoBusqueda = '';
  }

  filtrarProductos(): void {
    const q = this.busquedaProducto.toLowerCase();
    this.productosFiltrados.set(
      this.productos().filter(p =>
        (p.articulo?.nombre?.toLowerCase().includes(q)) ||
        (p.producto?.nombre?.toLowerCase().includes(q)) ||
        (p.almacen?.nombre?.toLowerCase().includes(q))
      )
    );
    this.cdr.markForCheck();
  }

  agregarAlCarrito(pa: ProductoAlmacen): void {
    if (!pa.stock || pa.stock <= 0) {
      alert('Sin stock disponible');
      return;
    }

    const nombreProducto = pa.articulo?.nombre || pa.producto?.nombre || 'Sin nombre';
    const precio = pa.articulo?.precio || pa.producto?.precio || 0;
    const idArticuloAlmacen = pa.id;

    const existente = this.carrito().find(i => i.idProducto === idArticuloAlmacen);

    if (existente) {
      existente.cantidad = (existente.cantidad || 0) + 1;
      existente.precioSubtotal = existente.cantidad * (existente.precioUni || 0);
    } else {
      const item: SaleDetail = {
        idProducto:     idArticuloAlmacen,
        nombreProducto: nombreProducto,
        idAlmacen:      pa.id_Almacen || pa.almacenId,
        nombreAlmacen:  pa.almacen?.nombre,
        cantidad:       1,
        precioUni:      precio,
        precioSubtotal: precio
      };
      this.carrito.update(c => [...c, item]);
    }

    this.calcularTotal();
    this.cdr.markForCheck();
  }

  quitarDelCarrito(index: number): void {
    this.carrito.update(c => c.filter((_, i) => i !== index));
    this.calcularTotal();
    this.cdr.markForCheck();
  }

  calcularTotal(): void {
    const t = this.carrito().reduce((sum, i) => sum + (i.precioSubtotal || 0), 0);
    this.total.set(t);
  }

  async crearVenta(): Promise<void> {
    const cliente = this.clienteSeleccionado();
    if (!cliente) { alert('Seleccione un cliente'); return; }
    if (this.carrito().length === 0) { alert('Agregue al menos un producto'); return; }

    this.cargando.set(true);

    const user = JSON.parse(sessionStorage.getItem('user') || '{}');

    const nuevaVenta: Sale = {
      clienteId: cliente.id!,
      tipoPago:  this.tipoPago,
      glosa:     this.glosa,
      usuarioId: user.userId || 1
    };

    try {
      // 1. Crear la venta
      const venta = await this.saleService.createSale(nuevaVenta).toPromise();
      if (!venta?.id) throw new Error('No se pudo crear la venta');

      this.ventaCreada.set(venta);

      // 2. Agregar cada detalle
      for (const item of this.carrito()) {
        await this.saleService.agregarDetalle(venta.id, item).toPromise();
      }
       
     // 2.5 Confirmar stock (publica evento a RabbitMQ → Inventario procesa)
        await this.http.post(`http://localhost:5000/api/venta/${venta.id}/confirmar-stock`, {}, { headers: this.headers }).toPromise();

      // 3. Si es QR → generar QR automáticamente
      if (this.tipoPago === 'qr') {
        this.ventaIdQr = venta.id;
        this.generarQrAutomatico(venta.id);
      } else {
        alert('¡Venta creada exitosamente!');
        this.router.navigate(['/dashboard/sale/list']);
      }

    } catch (error) {
      console.error('Error al crear venta:', error);
      alert('Error al crear la venta. Intente nuevamente.');
    } finally {
      this.cargando.set(false);
      this.cdr.markForCheck();
    }
  }

  generarQrAutomatico(idVenta: number): void {
    this.http.post<any>(
      `http://localhost:5000/api/venta/${idVenta}/generarQr`,
      {},
      { headers: this.headers }
    ).subscribe({
      next: (res) => {
        if (res.ok) {
          this.qrUrl.set(res.qrUrl);
          this.urlPasarela.set(res.urlPasarela);
          this.mostrarModalQr.set(true);
          this.iniciarPolling(idVenta);
        } else {
          alert('Venta creada pero no se pudo generar el QR: ' + res.mensaje);
          this.router.navigate(['/dashboard/sale/list']);
        }
        this.cdr.markForCheck();
      },
      error: () => {
        alert('Venta creada pero no se pudo generar el QR.');
        this.router.navigate(['/dashboard/sale/list']);
        this.cdr.markForCheck();
      }
    });
  }

  iniciarPolling(idVenta: number): void {
    this.pollingInterval = setInterval(() => {
      this.http.get<any>(
        `http://localhost:5000/api/venta/${idVenta}/estadoPago`,
        { headers: this.headers }
      ).subscribe(res => {
        if (res.estado === 'pagado') {
          this.detenerPolling();
          this.mostrarModalQr.set(false);
          this.cdr.markForCheck();
          this.router.navigate(['/dashboard/sale/list']);
        }
      });
    }, 5000);
  }

  detenerPolling(): void {
    if (this.pollingInterval) {
      clearInterval(this.pollingInterval);
      this.pollingInterval = null;
    }
  }

  cerrarModalQr(): void {
    this.detenerPolling();
    this.mostrarModalQr.set(false);
    this.router.navigate(['/dashboard/sale/list']);
  }
}