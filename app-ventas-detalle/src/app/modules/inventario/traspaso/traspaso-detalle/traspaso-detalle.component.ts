import { CommonModule, DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TraspasoService } from '../service/traspaso.service';

@Component({
  selector: 'app-traspaso-detalle',
  imports: [CommonModule, DatePipe],
  templateUrl: './traspaso-detalle.component.html',
  styleUrl: './traspaso-detalle.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TraspasoDetalleComponent implements OnInit {
  traspaso: any = null;
  detalles: any[] = [];
  username: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private traspasoService: TraspasoService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const user = JSON.parse(sessionStorage.getItem('user') || '{}');
    this.username = user.fullname || user.username || 'Usuario';

    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.traspasoService.getTraspasoById(id).subscribe({
      next: (data) => {
        this.traspaso = data;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });

    this.traspasoService.getDetallesByTraspaso(id).subscribe({
      next: (data) => {
        this.detalles = data;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });
  }

  volver(): void {
    this.router.navigate(['/dashboard/traspaso']);
  }
}