import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Sale } from '../../../interfaces/sale.interface';
import { User } from '../../../interfaces/user.interface';

@Component({
  selector: 'app-sale-detalle',
  imports: [CommonModule, RouterModule],
  templateUrl: './sale-detalle.component.html',
  styleUrl: './sale-detalle.component.css',
})
export class SaleDetalleComponent implements OnInit {

  venta: Sale | null = null;
  codigo: string = '';
  ventaId: number = 0;
  nombreUsuario: string = '—';

  private get headers(): HttpHeaders {
    const token = sessionStorage.getItem('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token ?? ''}`,
      'Content-Type': 'application/json'
    });
  }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.ventaId = +this.route.snapshot.paramMap.get('id')!;
    this.cargarVenta();
  }

  cargarVenta(): void {
    this.http.get<Sale>(`http://localhost:5000/api/venta/${this.ventaId}`, { headers: this.headers })
      .subscribe(data => {
        this.venta = data;
        this.codigo = `VTA-${String(data.id).padStart(5, '0')}`;
        if (data.usuarioId) {
          this.cargarUsuario(data.usuarioId);
        }
      });
  }

  cargarUsuario(usuarioId: number): void {
    this.http.get<User>(`http://localhost:5000/api/usuario/${usuarioId}`, { headers: this.headers })
      .subscribe(user => {
        this.nombreUsuario = user.fullname ?? '—';
      });
  }

  volver(): void {
    this.router.navigate(['/dashboard/sale/list']);
  }

  getEstadoClass(estado: string): string {
    switch (estado) {
      case 'pagado':    return 'text-green-400 font-semibold';
      case 'activo':    return 'text-blue-400 font-semibold';
      case 'cancelado': return 'text-red-400 font-semibold';
      default:          return 'text-gray-400';
    }
  }
}