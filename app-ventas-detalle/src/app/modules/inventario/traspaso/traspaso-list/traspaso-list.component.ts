import { CommonModule, DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { Traspaso } from '../../../../interfaces/traspaso.interface';
import { TraspasoService } from '../service/traspaso.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-traspaso-list',
  imports: [CommonModule, DatePipe],
  templateUrl: './traspaso-list.component.html',
  styleUrl: './traspaso-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TraspasoListComponent implements OnInit {
  @Input() public traspasos: Traspaso[] = [];
  public username: string = '';

  constructor(
    private traspasoService: TraspasoService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit(): void {
    const user = JSON.parse(sessionStorage.getItem('user') || '{}');
    this.username = user.fullname || user.username || 'Usuario';
  }

  reloadTraspasos(): void {
    this.traspasoService.getTraspasos().subscribe({
      next: (traspasos) => {
        this.traspasos = traspasos;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });
  }

  crearTraspaso() {
    this.router.navigate(['/dashboard/traspaso/add']);
  }

  verDetalle(id: number) {
    this.router.navigate(['/dashboard/traspaso/detalle', id]);
  }
}