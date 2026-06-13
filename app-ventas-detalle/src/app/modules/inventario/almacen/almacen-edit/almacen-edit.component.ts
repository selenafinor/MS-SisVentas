import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AlmacenService } from '../service/almacen.service';
import { Almacen } from '../../../../interfaces/almacen.interface';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-almacen-edit',
  imports: [CommonModule, FormsModule],
  templateUrl: './almacen-edit.component.html',
  styleUrl: './almacen-edit.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AlmacenEditComponent implements OnInit {
  almacen: Almacen = {};

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private almacenService: AlmacenService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.almacenService.getAlmacenById(id).subscribe({
      next: (data) => {
        this.almacen = data;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err),
    });
  }

  guardar(): void {
    this.almacenService.updateAlmacen(this.almacen.id!, this.almacen).subscribe({
      next: () => {
        Swal.fire('¡Actualizado!', 'Almacén actualizado correctamente.', 'success');
        this.router.navigate(['/dashboard/almacen']);
      },
      error: (err) => console.error(err),
    });
  }

  cancelar(): void {
    this.router.navigate(['/dashboard/almacen']);
  }
}