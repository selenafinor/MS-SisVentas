import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CustomerService } from '../service/customer.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-customer-add',
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './customer-add.component.html',
  styleUrl: './customer-add.component.css',
})
export default class CustomerAddComponent {

  nombre: string = '';
  paterno: string = '';
  materno: string = '';
  telefono: string = '';
  correo: string = '';
  nit: string = '';
  direccion: string = '';
  errorMessage: string = '';

  constructor(
    private customerService: CustomerService,
    private router: Router
  ) {}

  onSubmit(): void {
    if (!this.nombre) {
      this.errorMessage = 'El nombre es obligatorio';
      return;
    }
    const customer = {
      nombre:    this.nombre,
      paterno:   this.paterno,
      materno:   this.materno,
      telefono:  this.telefono,
      correo:    this.correo,
      nit:       this.nit,
      direccion: this.direccion,
      estado:    'activo'
    };
    this.customerService.createCustomer(customer).subscribe({
      next: () => {
        alert('Cliente creado exitosamente');
        this.router.navigate(['/dashboard/customer/list']);
      },
      error: () => {
        this.errorMessage = 'Error al crear el cliente';
      }
    });
  }
}