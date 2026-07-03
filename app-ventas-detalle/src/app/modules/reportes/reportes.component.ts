import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReporteService } from './service/reporte.service';
import {
  NotaVenta,
  NotaCompra,
  ArticuloReporte,
  Ingreso,
  Egreso,
  TopArticulo,
  DestinatarioCorreo
} from '../../interfaces/reporte.interface';

type TabReporte = 'ventas' | 'compras' | 'inventario' | 'movimientos';

@Component({
  selector: 'app-reportes',
  imports: [CommonModule, FormsModule],
  templateUrl: './reportes.component.html',
  styleUrl: './reportes.component.css'
})
export class ReportesComponent implements OnInit {
  tabActivo: TabReporte = 'ventas';

  // Filtros
  fechaDesde: string = this.primerDiaMes();
  fechaHasta: string = this.hoy();
  filtroEstado: string = 'todos';
  filtroTipoPago: string = 'todos';

  // Datos Ventas
  ventas: NotaVenta[] = [];
  totalVendido = 0;
  ventasCanceladas = 0;
  topArticulos: TopArticulo[] = [];

  // Datos Compras
  compras: NotaCompra[] = [];
  totalComprado = 0;

  // Datos Inventario
  articulos: ArticuloReporte[] = [];
  totalArticulos = 0;
  totalStockBajo = 0;

  // Datos Movimientos
  ingresos: Ingreso[] = [];
  egresos: Egreso[] = [];

  cargando = false;

  // Modal de correo
  modalCorreoAbierto = false;
  enviandoCorreo = false;

  // ─── NUEVO: lista de destinatarios en vez de uno solo ───
  destinatarios: DestinatarioCorreo[] = [];
  nombreDestinoTemp = '';
  correoDestinoTemp = '';

  constructor(
    private reporteService: ReporteService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.cargarTabActivo();
  }

  cambiarTab(tab: TabReporte): void {
    this.tabActivo = tab;
    this.cargarTabActivo();
  }

  cargarTabActivo(): void {
    switch (this.tabActivo) {
      case 'ventas':
        this.cargarVentas();
        break;
      case 'compras':
        this.cargarCompras();
        break;
      case 'inventario':
        this.cargarInventario();
        break;
      case 'movimientos':
        this.cargarMovimientos();
        break;
    }
  }

  aplicarFiltros(): void {
    this.cargarTabActivo();
  }

  // ─── VENTAS ───────────────────────────────────────────

  cargarVentas(): void {
    this.cargando = true;
    this.reporteService.getReporteVentas({
      desde: this.fechaDesde,
      hasta: this.fechaHasta,
      estado: this.filtroEstado !== 'todos' ? this.filtroEstado : undefined,
      tipoPago: this.filtroTipoPago !== 'todos' ? this.filtroTipoPago : undefined
    }).subscribe({
      next: (res) => {
        this.ventas = res.ventas;
        this.totalVendido = res.totalVendido;
        this.ventasCanceladas = res.ventasCanceladas;
        this.calcularTopArticulos();
        this.cargando = false;
        this.cdr.markForCheck();
      },
      error: (err) => {
        console.error('Error cargando ventas:', err);
        this.cargando = false;
      }
    });
  }

  calcularTopArticulos(): void {
    const conteo: { [nombre: string]: { cantidad: number; total: number } } = {};

    this.ventas.forEach(venta => {
      if (venta.detalles) {
        venta.detalles.forEach(d => {
          const nombre = d.nombreProducto || 'Sin nombre';
          if (!conteo[nombre]) {
            conteo[nombre] = { cantidad: 0, total: 0 };
          }
          conteo[nombre].cantidad += d.cantidad || 0;
          conteo[nombre].total += d.precioSubtotal || 0;
        });
      }
    });

    this.topArticulos = Object.keys(conteo)
      .map(nombre => ({
        nombre,
        cantidad: conteo[nombre].cantidad,
        total: conteo[nombre].total
      }))
      .sort((a, b) => b.cantidad - a.cantidad)
      .slice(0, 5);
  }

  getNombreCliente(venta: NotaVenta): string {
    if (!venta.cliente) return '—';
    return `${venta.cliente.nombre || ''} ${venta.cliente.paterno || ''}`.trim() || '—';
  }

  getArticulosVenta(venta: NotaVenta): string {
    if (!venta.detalles || venta.detalles.length === 0) return 'N/A';
    return venta.detalles.map(d => `${d.nombreProducto} x${d.cantidad}`).join(', ');
  }

  // ─── COMPRAS ──────────────────────────────────────────

  cargarCompras(): void {
    this.cargando = true;
    this.reporteService.getReporteCompras({
      desde: this.fechaDesde,
      hasta: this.fechaHasta,
      estado: this.filtroEstado !== 'todos' ? this.filtroEstado : undefined
    }).subscribe({
      next: (res) => {
        this.compras = res.compras;
        this.totalComprado = res.totalComprado;
        this.cargando = false;
        this.cdr.markForCheck();
      },
      error: (err) => {
        console.error('Error cargando compras:', err);
        this.cargando = false;
      }
    });
  }

  getNombreProveedor(compra: NotaCompra): string {
    return compra.proveedor?.nombre || '—';
  }

  // ─── INVENTARIO ───────────────────────────────────────

  cargarInventario(): void {
    this.cargando = true;
    this.reporteService.getReporteInventario().subscribe({
      next: (res) => {
        this.articulos = res.articulos;
        this.totalArticulos = res.totalArticulos;
        this.totalStockBajo = res.totalStockBajo;
        this.cargando = false;
        this.cdr.markForCheck();
      },
      error: (err) => {
        console.error('Error cargando inventario:', err);
        this.cargando = false;
      }
    });
  }

  esStockBajo(articulo: ArticuloReporte): boolean {
    return (articulo.stock || 0) <= (articulo.stockMin || 0);
  }

  // ─── MOVIMIENTOS ──────────────────────────────────────

  cargarMovimientos(): void {
    this.cargando = true;
    this.reporteService.getReporteMovimientos({
      desde: this.fechaDesde,
      hasta: this.fechaHasta
    }).subscribe({
      next: (res) => {
        this.ingresos = res.ingresos;
        this.egresos = res.egresos;
        this.cargando = false;
        this.cdr.markForCheck();
      },
      error: (err) => {
        console.error('Error cargando movimientos:', err);
        this.cargando = false;
      }
    });
  }

  getArticulosMovimiento(detalles: any[] | undefined): string {
    if (!detalles || detalles.length === 0) return '—';
    return detalles
      .map(d => `${d.articuloAlmacen?.articulo?.nombre || '—'} x${d.cantidad}`)
      .join(', ');
  }

  // ─── DESCARGAR PDF ────────────────────────────────────

  descargarPdf(): void {
    this.cargando = true;
    let observable;

    switch (this.tabActivo) {
      case 'ventas':
        observable = this.reporteService.descargarPdfVentas({
          desde: this.fechaDesde,
          hasta: this.fechaHasta,
          estado: this.filtroEstado !== 'todos' ? this.filtroEstado : undefined,
          tipoPago: this.filtroTipoPago !== 'todos' ? this.filtroTipoPago : undefined
        });
        break;
      case 'compras':
        observable = this.reporteService.descargarPdfCompras({
          desde: this.fechaDesde,
          hasta: this.fechaHasta,
          estado: this.filtroEstado !== 'todos' ? this.filtroEstado : undefined
        });
        break;
      case 'inventario':
        observable = this.reporteService.descargarPdfInventario();
        break;
      case 'movimientos':
        observable = this.reporteService.descargarPdfMovimientos({
          desde: this.fechaDesde,
          hasta: this.fechaHasta
        });
        break;
    }

    observable.subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `reporte-${this.tabActivo}-${this.hoy()}.pdf`;
        a.click();
        window.URL.revokeObjectURL(url);
        this.cargando = false;
      },
      error: (err) => {
        console.error('Error descargando PDF:', err);
        this.cargando = false;
        alert('Error al descargar el PDF');
      }
    });
  }

  // ─── ENVIAR POR CORREO ────────────────────────────────

  abrirModalCorreo(): void {
    this.modalCorreoAbierto = true;
  }

  cerrarModalCorreo(): void {
    this.modalCorreoAbierto = false;
    this.destinatarios = [];
    this.nombreDestinoTemp = '';
    this.correoDestinoTemp = '';
  }

  // Agrega un destinatario a la lista (botón "+" del modal)
  agregarDestinatario(): void {
    if (!this.correoDestinoTemp || !this.nombreDestinoTemp) {
      alert('Completa el nombre y correo antes de agregar');
      return;
    }

    // Evita duplicados por correo
    const yaExiste = this.destinatarios.some(
      d => d.correo.toLowerCase() === this.correoDestinoTemp.toLowerCase()
    );
    if (yaExiste) {
      alert('Ese correo ya fue agregado');
      return;
    }

    this.destinatarios.push({
      nombre: this.nombreDestinoTemp,
      correo: this.correoDestinoTemp
    });

    this.nombreDestinoTemp = '';
    this.correoDestinoTemp = '';
  }

  quitarDestinatario(index: number): void {
    this.destinatarios.splice(index, 1);
  }

  enviarCorreo(): void {
    // Si el usuario dejó algo escrito en los campos temporales sin
    // presionar "+", lo agregamos automáticamente antes de enviar.
    if (this.correoDestinoTemp && this.nombreDestinoTemp) {
      this.agregarDestinatario();
    }

    if (this.destinatarios.length === 0) {
      alert('Agrega al menos un destinatario');
      return;
    }

    const usuarioLogueado = JSON.parse(sessionStorage.getItem('user') || 'null');

    this.enviandoCorreo = true;
    this.reporteService.enviarPorCorreo({
      destinatarios: this.destinatarios,
      tipoReporte: this.tabActivo,
      remitenteNombre: usuarioLogueado?.fullname || usuarioLogueado?.username,
      remitenteEmail: usuarioLogueado?.correo
    }).subscribe({
      next: () => {
        alert('Correo enviado correctamente');
        this.enviandoCorreo = false;
        this.cerrarModalCorreo();
      },
      error: (err) => {
        console.error('Error enviando correo:', err);
        alert('Error al enviar el correo');
        this.enviandoCorreo = false;
      }
    });
  }

  // ─── HELPERS DE FECHA ─────────────────────────────────

  private hoy(): string {
    return new Date().toISOString().split('T')[0];
  }

  private primerDiaMes(): string {
    const d = new Date();
    return new Date(d.getFullYear(), d.getMonth(), 1).toISOString().split('T')[0];
  }
}