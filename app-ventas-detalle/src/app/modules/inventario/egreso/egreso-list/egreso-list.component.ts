import { CommonModule, DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { Egreso } from '../../../../interfaces/egreso.interface';
import { EgresoService } from '../service/egreso.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-egreso-list',
  imports: [CommonModule, DatePipe],
  templateUrl: './egreso-list.component.html',
  styleUrl: './egreso-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EgresoListComponent implements OnInit {
  @Input() public egresos: Egreso[] = [];
  public username: string = '';

  constructor(
    private egresoService: EgresoService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit(): void {
    const user = JSON.parse(sessionStorage.getItem('user') || '{}');
    this.username = user.fullname || user.username || 'Usuario';
  }

  reloadEgresos(): void {
    this.egresoService.getEgresos().subscribe({
      next: (egresos) => {
        this.egresos = egresos;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });
  }

  crearEgreso() {
    this.router.navigate(['/dashboard/egreso/add']);
  }

  verDetalle(id: number) {
    this.router.navigate(['/dashboard/egreso/detalle', id]);
  }
}