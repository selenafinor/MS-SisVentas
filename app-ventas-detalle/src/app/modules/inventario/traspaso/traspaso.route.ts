import { Routes } from '@angular/router';
import { TraspasoComponent } from './traspaso.component';
import { TraspasoAddComponent } from './traspaso-add/traspaso-add.component';
import { TraspasoDetalleComponent } from './traspaso-detalle/traspaso-detalle.component';

export const traspaso_routes: Routes = [
  {
    path: '',
    component: TraspasoComponent,
  },
  {
    path: 'add',
    component: TraspasoAddComponent,
  },
  {
    path: 'detalle/:id',
    component: TraspasoDetalleComponent,
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full',
  },
];