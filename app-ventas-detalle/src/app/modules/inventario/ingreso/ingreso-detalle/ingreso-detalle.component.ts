import { CommonModule, DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IngresoService } from '../service/ingreso.service';

@Component({
  selector: 'app-ingreso-detalle',
  imports: [CommonModule, DatePipe],
  templateUrl: './ingreso-detalle.component.html',
  styleUrl: './ingreso-detalle.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class IngresoDetalleComponent implements OnInit {
  ingreso: any = null;
  detalles: any[] = [];
  username: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private ingresoService: IngresoService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const user = JSON.parse(sessionStorage.getItem('user') || '{}');
    this.username = user.fullname || user.username || 'Usuario';

    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.ingresoService.getIngresoById(id).subscribe({
      next: (data) => {
        this.ingreso = data;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });

    this.ingresoService.getDetallesByIngreso(id).subscribe({
      next: (data) => {
        this.detalles = data;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });
  }

  volver(): void {
    this.router.navigate(['/dashboard/ingreso']);
  }
}