import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { Rol } from '../../../interfaces/rol.interface';
import { Permiso } from '../../../interfaces/permiso.interface';
import { User } from '../../../interfaces/user.interface';

@Component({
  selector: 'app-dashboard-layout',
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboardLayout.component.html',
  styleUrl: './dashboardLayout.component.css',
changeDetection: ChangeDetectionStrategy.Default,
})
export class DashboardLayoutComponent implements OnInit {
  public userPermissions: Permiso[] = [];
  user: User | undefined;

  generalRoutes = [
    { path: 'user', title: 'Usuarios', icon: 'pi pi-users', permission: 'gestionar_usuarios' },
    { path: 'roles', title: 'Roles', icon: 'pi pi-shield', permission: 'gestionar_roles' },
    { path: 'permisos', title: 'Permisos', icon: 'pi pi-key', permission: 'gestionar_permisos' },
  ];

  inventarioRoutes = [
    { path: 'articulo', title: 'Artículos', permission: 'gestionar_inventario' },
    { path: 'almacen', title: 'Almacenes', permission: 'gestionar_inventario' },
    { path: 'ingreso', title: 'Ingresos', permission: 'gestionar_inventario' },
    { path: 'egreso', title: 'Egresos', permission: 'gestionar_inventario' },
    { path: 'traspaso', title: 'Traspasos', permission: 'gestionar_inventario' },
  ];

  ventasRoutes = [
    { path: 'customer', title: 'Clientes', permission: 'gestionar_ventas' },
    { path: 'sale', title: 'Ventas', permission: 'gestionar_ventas' },
  ];

  comprasRoutes = [
  { path: 'compra/proveedor/list', title: 'Proveedores', permission: 'gestionar_compras' },
];

  inventarioOpen = false;
  ventasOpen = false;
  comprasOpen = false;

  constructor(private router: Router, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    const permissions = JSON.parse(sessionStorage.getItem('roles') || '[]');
    const user = JSON.parse(sessionStorage.getItem('user') || '[]');
    this.user = user;
    this.userPermissions = permissions.flatMap((rol: Rol) => rol.permisos);
  }

  hasPermission(permissionName: string): boolean {
    return this.userPermissions.some(perm => perm.nombre_Permiso === permissionName);
  }

  hasAnyPermission(routes: any[]): boolean {
    return routes.some(r => this.hasPermission(r.permission));
  }

  logout() {
    sessionStorage.removeItem('user');
    sessionStorage.removeItem('roles');
    sessionStorage.removeItem('token');
    this.router.navigate(['/']);
  }
}