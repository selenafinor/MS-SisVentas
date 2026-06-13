import { Routes } from '@angular/router';
import { IngresoComponent } from './ingreso.component';
import { IngresoAddComponent } from './ingreso-add/ingreso-add.component';
import { IngresoDetalleComponent } from './ingreso-detalle/ingreso-detalle.component';

export const ingreso_routes: Routes = [
  {
    path: '',
    component: IngresoComponent,
  },
  {
    path: 'add',
    component: IngresoAddComponent,
  },
  {
    path: 'detalle/:id',
    component: IngresoDetalleComponent,
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full',
  },
];