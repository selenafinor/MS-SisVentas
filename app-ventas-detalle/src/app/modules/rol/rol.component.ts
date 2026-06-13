import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { RolListComponent } from './rol-list/rol-list.component';
import { RolService } from './rol.service';

@Component({
  selector: 'app-rol',
  imports: [RolListComponent],
  templateUrl: './rol.component.html',
  styleUrl: './rol.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RolComponent implements OnInit {
  roles: any[] = [];
  errorMessage: string | null = null;

  constructor(
    private rolService: RolService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadRol();
  }

  loadRol(): void {
    this.rolService.getRolesConPermisos().subscribe({
      next: (data) => {
        this.roles = data;
        this.cdr.markForCheck();
      },
      error: () => {
        this.errorMessage = 'Hubo un error al cargar los roles';
        this.cdr.markForCheck();
      }
    });
  }
}