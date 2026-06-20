import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { CustomerService } from './service/customer.service';
import { Customer } from '../../interfaces/customer.interface';

export const customerEditResolver: ResolveFn<Customer> = (route) => {
  const id = +route.paramMap.get('id')!;
  return inject(CustomerService).getCustomerById(id);
};