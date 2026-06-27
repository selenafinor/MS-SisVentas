//rutas de compra
import { Routes } from '@angular/router';
import ProveedorAddComponent from './proveedor/proveedor-add/proveedor-add.component';
import { ProveedorListComponent } from './proveedor/proveedor-list/proveedor-list.component';
import { ProveedorEditComponent } from './proveedor/proveedor-edit/proveedor-edit.component';
import { proveedorEditResolver } from './proveedor-edit.resolver';
import { OrdenCompraListComponent } from './orden_compra/orden-compra-list/orden-compra-list.component';
import { OrdenCompraAddComponent } from './orden_compra/orden-compra-add/orden-compra-add.component';
import { CatalogoComponent } from './catalogo/catalogo.component';
import { AdquisicionComponent } from './adquisicion/adquisicion.component';
import { NotaCompraComponent } from './nota-compra/nota-compra.component';
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
  {
    path: 'catalogo',
    component: CatalogoComponent
  },
  {
    path: 'orden-compra',
    children: [
      { path: 'list', component: OrdenCompraListComponent },
      { path: 'add', component: OrdenCompraAddComponent },
      { path: '**', redirectTo: 'list', pathMatch: 'full' },
    ]
  },
  { path: 'adquisicion', component: AdquisicionComponent },
  { path: 'nota-compra', component: NotaCompraComponent },
  { path: '**', redirectTo: 'proveedor/list', pathMatch: 'full' },
];