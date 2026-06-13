import { Routes } from '@angular/router';
import { PermisosComponent } from './permisos.component';
import { PermisoAddComponent } from './permiso-add/permiso-add.component';

export const permisos_routes: Routes = [
  {
    path: '',
    component: PermisosComponent,
  },
  {
    path: 'add',
    component: PermisoAddComponent,
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full',
  },
];