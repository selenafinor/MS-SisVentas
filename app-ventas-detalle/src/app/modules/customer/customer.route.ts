import { Routes } from '@angular/router';
import CustomerAddComponent from './customer-add/customer-add.component';
import { CustomerListComponent } from './customer-list/customer-list.component';
import { CustomerEditComponent } from './customer-edit/customer-edit.component';
import { customerEditResolver } from './customer-edit.resolver';

export const customer_routes: Routes = [
  { path: 'list', component: CustomerListComponent },
  { path: 'add', component: CustomerAddComponent },
  { 
    path: 'edit/:id', 
    component: CustomerEditComponent,
    resolve: { cliente: customerEditResolver }
  },
  { path: '**', redirectTo: 'list', pathMatch: 'full' },
];