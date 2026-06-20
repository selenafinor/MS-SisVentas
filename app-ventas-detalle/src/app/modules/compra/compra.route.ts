import { Routes } from '@angular/router';
import ProveedorAddComponent from './proveedor-add/proveedor-add.component';
import { ProveedorListComponent } from './proveedor-list/proveedor-list.component';
import { ProveedorEditComponent } from './proveedor-edit/proveedor-edit.component';
import { proveedorEditResolver } from './proveedor-edit.resolver';

export const compra_routes: Routes = [
  {
    path: 'proveedor',
    children: [
      { path: 'list', component: ProveedorListComponent },
      { path: 'add', component: ProveedorAddComponent },
      {
        path: 'edit/:id',
        component: ProveedorEditComponent,
        resolve: { proveedor: proveedorEditResolver }
      },
      { path: '**', redirectTo: 'list', pathMatch: 'full' },
    ]
  },
  { path: '**', redirectTo: 'proveedor/list', pathMatch: 'full' },
];