import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CustomerService } from '../service/customer.service';

@Component({
  selector: 'app-customer-edit',
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './customer-edit.component.html',
  styleUrl: './customer-edit.component.css',
})
export class CustomerEditComponent implements OnInit {

  id: number = 0;
  nombre: string = '';
  paterno: string = '';
  materno: string = '';
  telefono: string = '';
  correo: string = '';
  nit: string = '';
  direccion: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private customerService: CustomerService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const data = this.route.snapshot.data['cliente'];
    console.log('Resolver data:', data);
    this.id        = data.id        ?? 0;
    this.nombre    = data.nombre    ?? '';
    this.paterno   = data.paterno   ?? '';
    this.materno   = data.materno   ?? '';
    this.telefono  = data.telefono  ?? '';
    this.correo    = data.correo    ?? '';
    this.nit       = data.nit       ?? '';
    this.direccion = data.direccion ?? '';
    this.cdr.detectChanges();
  }

  onSubmit(): void {
    const customerData = {
      id: this.id,
      nombre: this.nombre,
      paterno: this.paterno,
      materno: this.materno,
      telefono: this.telefono,
      correo: this.correo,
      nit: this.nit,
      direccion: this.direccion
    };
    this.customerService.updateCustomer(customerData).subscribe({
      next: () => {
        alert('Cliente actualizado exitosamente.');
        this.router.navigate(['/dashboard/customer/list']);
      },
      error: () => alert('Error al actualizar el cliente.')
    });
  }
}