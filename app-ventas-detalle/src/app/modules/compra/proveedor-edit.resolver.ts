import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { ProveedorService } from './service/proveedor.service';
import { Proveedor } from '../../interfaces/proveedor.interface';

export const proveedorEditResolver: ResolveFn<Proveedor> = (route) => {
  const id = +route.paramMap.get('id')!;
  return inject(ProveedorService).getProveedorById(id);
};