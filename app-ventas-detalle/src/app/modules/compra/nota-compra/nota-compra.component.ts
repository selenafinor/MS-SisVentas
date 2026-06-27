import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { NotaCompraService } from '../service/nota-compra.service';
import { AdquisicionService } from '../service/adquisicion.service';
import { OrdenCompraService } from '../service/orden-compra.service';
import { AlmacenService } from '../../inventario/almacen/service/almacen.service';
import { NotaCompra } from '../../../interfaces/nota-compra.interface';
import { OrdenCompra } from '../../../interfaces/orden-compra.interface';
import { Almacen } from '../../../interfaces/almacen.interface';

@Component({
  selector: 'app-nota-compra',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './nota-compra.component.html',
  styleUrl: './nota-compra.component.css',
  changeDetection: ChangeDetectionStrategy.Default,
})
export class NotaCompraComponent implements OnInit {

  mostrarFormulario: boolean = false;

  notas: NotaCompra[] = [];
  almacenes: Almacen[] = [];
  ordenDesdeAdq: OrdenCompra | null = null;
  filaExpandida: number | null = null;

  almacenId: string = '';
  tipoPago: string = 'efectivo';
  nroTransaccion: string = '';
  glosa: string = '';

  procesando = signal<boolean>(false);
  completado = false;
  mensaje: string = '';

  constructor(
    private notaService: NotaCompraService,
    private adquisicionService: AdquisicionService,
    private ordenService: OrdenCompraService,
    private almacenService: AlmacenService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const ordenId = this.route.snapshot.queryParamMap.get('ordenId');
    if (ordenId) {
      this.ordenService.getById(+ordenId).subscribe(orden => {
        if (orden?.id) {
          this.ordenDesdeAdq = orden;
          this.mostrarFormulario = true;
        }
      });
    }

    this.notaService.getAll().subscribe(data => {
      this.notas = data;
    });
    this.almacenService.getAlmacenes().subscribe(data => {
      this.almacenes = data.filter(a => a.estado === 'activo');
    });
  }

  async confirmarDesdeOrden(): Promise<void> {
    if (!this.almacenId) {
      this.mensaje = 'Debes seleccionar un almacén destino';
      return;
    }
    const orden = this.ordenDesdeAdq!;
    this.procesando.set(true);
    this.mensaje = '';

    const user = JSON.parse(sessionStorage.getItem('user') || '{}');

    try {
      // 1. Crear nota de compra
      const nota = await this.notaService.create({
        proveedorId: orden.proveedorId,
        glosa: this.glosa || `Nota de compra — Orden #${orden.id}`,
        tipoPago: this.tipoPago,
        usuarioId: user.userId || 1
      }).toPromise();
      if (!nota?.id) throw new Error('No se pudo crear la nota de compra');

      // 2. Agregar detalles a la nota
      for (const d of orden.detalles || []) {
        await this.notaService.agregarDetalle(nota.id, {
          productoId: d.productoId,
          nombreProducto: d.nombreProducto,
          cantidad: d.cantidad,
          precioUni: d.precioUni
        }).toPromise();
      }

      // 3. Confirmar nota (crea Ingreso histórico soloRegistro=true)
      await this.notaService.confirmarStock(nota.id).toPromise();

      // 4. Crear adquisición vinculada a la orden
      const adq = await this.adquisicionService.create({
        proveedorId: orden.proveedorId,
        ordenId: orden.id,
        glosa: `Recepción — Nota #${nota.id}`,
        usuarioId: user.userId || 1
      }).toPromise();
      if (!adq?.id) throw new Error('No se pudo crear la adquisición');

      // 5. Agregar detalles a la adquisición (con almacén destino)
      for (const d of orden.detalles || []) {
        await this.adquisicionService.agregarDetalle(adq.id, {
          productoId: d.productoId,
          almacenId: +this.almacenId,
          nombreProducto: d.nombreProducto,
          cantidad: d.cantidad,
          precioUni: d.precioUni
        }).toPromise();
      }

      // 6. Confirmar stock (RabbitMQ → suma stock + precio promedio ponderado)
      await this.adquisicionService.confirmarStock(adq.id).toPromise();

      // 7. Marcar orden como recibida
      await this.ordenService.actualizarEstado(orden.id!, 'recibido').toPromise();

      this.completado = true;
      this.mensaje = '✓ Nota de compra generada, ingreso registrado y stock actualizado correctamente.';
      this.notaService.getAll().subscribe(data => {
        this.notas = data;
      });

    } catch (err: any) {
      this.mensaje = 'Error: ' + (err.message || 'Ocurrió un problema');
    } finally {
      this.procesando.set(false);
    }
  }

  volver(): void {
    this.router.navigate(['/dashboard/compra/adquisicion']);
  }

  toggleFila(id: number): void {
    this.filaExpandida = this.filaExpandida === id ? null : id;
  }
}