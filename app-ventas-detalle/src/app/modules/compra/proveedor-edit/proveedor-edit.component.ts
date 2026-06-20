import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProveedorService } from '../service/proveedor.service';

@Component({
  selector: 'app-proveedor-edit',
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './proveedor-edit.component.html',
  styleUrl: './proveedor-edit.component.css',
})
export class ProveedorEditComponent implements OnInit {

  id: number = 0;
  nombre: string = '';
  contacto: string = '';
  telefono: string = '';
  email: string = '';
  nit: string = '';
  direccion: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private proveedorService: ProveedorService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const data = this.route.snapshot.data['proveedor'];
    this.id        = data.id        ?? 0;
    this.nombre    = data.nombre    ?? '';
    this.contacto  = data.contacto  ?? '';
    this.telefono  = data.telefono  ?? '';
    this.email     = data.email     ?? '';
    this.nit       = data.nit       ?? '';
    this.direccion = data.direccion ?? '';
    this.cdr.detectChanges();
  }

  onSubmit(): void {
    const proveedorData = {
      id: this.id,
      nombre: this.nombre,
      contacto: this.contacto,
      telefono: this.telefono,
      email: this.email,
      nit: this.nit,
      direccion: this.direccion
    };
    this.proveedorService.updateProveedor(proveedorData).subscribe({
      next: () => {
        alert('Proveedor actualizado exitosamente.');
        this.router.navigate(['/dashboard/compra/proveedor/list']);
      },
      error: () => alert('Error al actualizar el proveedor.')
    });
  }
}