import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CustomerService } from '../service/customer.service';
import { Customer } from '../../../interfaces/customer.interface';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-customer-list',
  imports: [CommonModule, RouterModule],
  templateUrl: './customer-list.component.html',
  styleUrl: './customer-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerListComponent implements OnInit {
  customers: Customer[] = [];
  errorMessage: string = '';

  constructor(
    private customerService: CustomerService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadCustomers();
  }

  loadCustomers(): void {
    this.customerService.getCustomerAll().subscribe({
      next: (data) => {
        if (Array.isArray(data) && data.length > 0) {
          this.customers = data;
          this.errorMessage = '';
        } else {
          this.customers = [];
          this.errorMessage = 'No se encontraron clientes disponibles.';
        }
        this.cdr.markForCheck();
      },
      error: () => {
        this.errorMessage = 'Error al cargar los clientes.';
        this.customers = [];
        this.cdr.markForCheck();
      }
    });
  }

  toggleEstado(id: number): void {
    this.customerService.toggleEstado(id).subscribe(() => {
      this.loadCustomers();
    });
  }

  deleteCustomer(id: number): void {
    if (confirm('¿Estás seguro de que deseas eliminar este cliente?')) {
      this.customerService.deleteCustomer(id).subscribe(() => {
        this.loadCustomers();
      });
    }
  }
}