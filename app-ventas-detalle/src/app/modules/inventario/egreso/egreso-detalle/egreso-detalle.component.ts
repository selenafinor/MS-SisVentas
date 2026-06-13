import { CommonModule, DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EgresoService } from '../service/egreso.service';

@Component({
  selector: 'app-egreso-detalle',
  imports: [CommonModule, DatePipe],
  templateUrl: './egreso-detalle.component.html',
  styleUrl: './egreso-detalle.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EgresoDetalleComponent implements OnInit {
  egreso: any = null;
  detalles: any[] = [];
  username: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private egresoService: EgresoService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const user = JSON.parse(sessionStorage.getItem('user') || '{}');
    this.username = user.fullname || user.username || 'Usuario';

    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.egresoService.getEgresoById(id).subscribe({
      next: (data) => {
        this.egreso = data;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });

    this.egresoService.getDetallesByEgreso(id).subscribe({
      next: (data) => {
        this.detalles = data;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });
  }

  volver(): void {
    this.router.navigate(['/dashboard/egreso']);
  }
}