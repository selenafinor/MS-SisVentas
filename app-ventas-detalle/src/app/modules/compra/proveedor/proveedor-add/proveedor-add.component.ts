import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ProveedorService } from '../../service/proveedor.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-proveedor-add',
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './proveedor-add.component.html',
  styleUrl: './proveedor-add.component.css',
})
export default class ProveedorAddComponent {

  nombre: string = '';
  contacto: string = '';
  telefono: string = '';
  email: string = '';
  nit: string = '';
  direccion: string = '';
  errorMessage: string = '';

  constructor(
    private proveedorService: ProveedorService,
    private router: Router
  ) {}

  onSubmit(): void {
    if (!this.nombre) {
      this.errorMessage = 'El nombre es obligatorio';
      return;
    }
    const proveedor = {
      nombre:    this.nombre,
      contacto:  this.contacto,
      telefono:  this.telefono,
      email:     this.email,
      nit:       this.nit,
      direccion: this.direccion,
      estado:    'activo'
    };
    this.proveedorService.createProveedor(proveedor).subscribe({
      next: () => {
        alert('Proveedor creado exitosamente');
        this.router.navigate(['/dashboard/compra/proveedor/list']);
      },
      error: () => {
        this.errorMessage = 'Error al crear el proveedor';
      }
    });
  }
}