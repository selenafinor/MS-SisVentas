import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Traspaso } from '../../../interfaces/traspaso.interface';
import { TraspasoService } from './service/traspaso.service';
import { TraspasoListComponent } from './traspaso-list/traspaso-list.component';

@Component({
  selector: 'app-traspaso',
  imports: [CommonModule, TraspasoListComponent],
  templateUrl: './traspaso.component.html',
  styleUrl: './traspaso.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TraspasoComponent implements OnInit {
  traspasos: Traspaso[] = [];
  errorMessage: string | null = null;

  constructor(
    private traspasoService: TraspasoService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadTraspasos();
  }

  loadTraspasos(): void {
    this.traspasoService.getTraspasos().subscribe({
      next: (data) => {
        this.traspasos = data;
        this.cdr.markForCheck();
      },
      error: () => {
        this.errorMessage = 'Error al cargar traspasos';
        this.cdr.markForCheck();
      }
    });
  }
}