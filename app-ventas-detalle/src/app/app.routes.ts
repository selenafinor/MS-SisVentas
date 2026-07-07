import { Routes } from '@angular/router';
import { DashboardLayoutComponent } from './shared/layouts/dashboardLayout/dashboardLayout.component';

export const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () =>
      import('./modules/auth/auth.route').then(m => m.auth_routes),
  },
  {
    path: 'dashboard',
    component: DashboardLayoutComponent,
    children: [
      {
        path: 'inicio',
        loadChildren: () =>
          import('./modules/dashboard-resumen/dashboard-resumen.route').then(m => m.dashboard_resumen_routes),
        data: { icon: 'pi pi-home', title: 'Inicio', description: 'Resumen general del sistema', permission: 'Dashboard' }
      },
      {
        path: 'roles',
        loadChildren: () =>
          import('./modules/rol/rol.route').then(m => m.rol_routes),
        data: { icon: 'pi pi-users', title: 'Roles', description: 'Gestion de Roles', permission: 'Rol' }
      },
      {
        path: 'roles-permisos',
        loadChildren: () =>
          import('./modules/roles-permisos/roles-permisos.route').then(m => m.roles_permisos_routes),
        data: { icon: 'pi pi-users', title: 'Asignacion de Permisos a los Roles', description: 'Gestion de Asignacion de Permisos a los Roles', permission: 'Rol Permiso' }
      },
      {
        path: 'user',
        loadChildren: () =>
          import('./modules/usuario/usuario.route').then(m => m.usuario_routes),
        data: { icon: 'pi pi-users', title: 'Usuario', description: 'Gestion de Usuarios', permission: 'Usuario' }
      },
      {
        path: 'roles-permisos-usuario',
        loadChildren: () =>
          import('./modules/roles-permisos-usuario/roles-permisos-usuario.route').then(m => m.roles_permisos_usuario_routes),
        data: { icon: 'pi pi-users', title: 'Asignacion de Roles a Usuarios', description: 'Gestion de Asignacion de Roles a los Usuario', permission: 'Asignacion Roles y Permisos' }
      },
      {
        path: 'permisos',
        loadChildren: () =>
          import('./modules/permisos/permiso.route').then(m => m.permisos_routes),
        data: { icon: 'pi pi-key', title: 'Permisos', description: 'Gestion de Permisos', permission: 'Permiso' }
      },
      {
        path: 'customer',
        loadChildren: () =>
          import('./modules/customer/customer.route').then(m => m.customer_routes),
        data: { icon: 'pi pi-users', title: 'Cliente', description: 'Gestion de Clientes', permission: 'Cliente' }
      },
      {
        path: 'category',
        loadChildren: () =>
          import('./modules/inventario/category/category.route').then(m => m.category_routes),
        data: { icon: 'pi pi-cart-plus', title: 'Categoria', description: 'Gestion de Categoria', permission: 'Categoria' }
      },
      {
        path: 'articulo',
        loadChildren: () =>
          import('./modules/inventario/articulo/product.route').then(m => m.product_routes),
        data: { icon: 'pi pi-cart-plus', title: 'Articulo', description: 'Gestion de Articulos', permission: 'Articulo' }
      },
      {
        path: 'asignar-producto',
        loadChildren: () =>
          import('./modules/inventario/AsignarProducto/asignar-producto.route').then(m => m.asignar_producto_routes),
        data: { icon: 'pi pi-file-pdf', title: 'Asignar Producto a Almacen', description: 'Gestion de Asignar Producto a Almacen', permission: 'Producto Almacen' }
      },
      {
        path: 'almacen',
        loadChildren: () =>
          import('./modules/inventario/almacen/almacen.route').then(m => m.almacen_routes),
        data: { icon: 'pi pi-file-pdf', title: 'Almacen', description: 'Gestion de Almacen', permission: 'Almacen' }
      },
      {
        path: 'sale',
        loadChildren: () =>
          import('./modules/sale/sale.route').then(m => m.sale_routes),
        data: { icon: 'pi pi-cart-plus', title: 'Venta', description: 'Gestion de Ventas', permission: 'Venta' }
      },
      {
        path: 'compra',
        loadChildren: () =>
          import('./modules/compra/compra.route').then(m => m.compra_routes),
        data: { icon: 'pi pi-truck', title: 'Compras', description: 'Gestion de Compras', permission: 'Compra' }
      },
      {
        path: 'ingreso',
        loadChildren: () =>
          import('./modules/inventario/ingreso/ingreso.route').then(m => m.ingreso_routes),
        data: { icon: 'pi pi-file-pdf', title: 'Ingreso', description: 'Gestion de Ingresos', permission: 'Ingreso' }
      },
      {
        path: 'egreso',
        loadChildren: () =>
          import('./modules/inventario/egreso/egreso.route').then(m => m.egreso_routes),
        data: { icon: 'pi pi-file-pdf', title: 'Egreso', description: 'Gestion de Egresos', permission: 'Egreso' }
      },
      {
        path: 'traspaso',
        loadChildren: () =>
          import('./modules/inventario/traspaso/traspaso.route').then(m => m.traspaso_routes),
        data: { icon: 'pi pi-file-pdf', title: 'Traspaso', description: 'Gestion de Traspasos', permission: 'Traspaso' }
      },
      {
        path: 'reportes',
        loadChildren: () =>
          import('./modules/reportes/reportes.route').then(m => m.reportes_routes),
        data: { icon: 'pi pi-chart-bar', title: 'Reportes', description: 'Gestion de Reportes', permission: 'ver_reportes' }
      },
    ],
  },
  {
    path: '**',
    redirectTo: 'auth',
    pathMatch: 'full',
  },
];