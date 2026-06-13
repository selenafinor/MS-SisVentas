import { Routes } from '@angular/router';
import { EgresoComponent } from './egreso.component';
import { EgresoAddComponent } from './egreso-add/egreso-add.component';
import { EgresoDetalleComponent } from './egreso-detalle/egreso-detalle.component';

export const egreso_routes: Routes = [
  {
    path: '',
    component: EgresoComponent,
  },
  {
    path: 'add',
    component: EgresoAddComponent,
  },
  {
    path: 'detalle/:id',
    component: EgresoDetalleComponent,
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full',
  },
];