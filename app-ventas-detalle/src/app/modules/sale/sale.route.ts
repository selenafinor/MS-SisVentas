import { Routes } from '@angular/router';
import { SaleAddComponent } from './sale-add/sale-add.component';
import { SaleListComponent } from './sale-list/sale-list.component';
import { SaleDetalleComponent } from './sale-detalle/sale-detalle.component';

export const sale_routes: Routes = [
  {
    path: 'list',
    component: SaleListComponent,
  },
  {
    path: 'add',
    component: SaleAddComponent
  },
  {
    path: 'detalle/:id',
    component: SaleDetalleComponent
  },
  {
    path: '**',
    redirectTo: 'list',
    pathMatch: 'full',
  },
];