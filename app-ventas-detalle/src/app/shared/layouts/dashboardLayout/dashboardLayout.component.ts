import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { routes } from '../../../app.routes';
import { Rol } from '../../../interfaces/rol.interface';
import { Permiso } from '../../../interfaces/permiso.interface';
import { User } from '../../../interfaces/user.interface';

@Component({
  selector: 'app-dashboard-layout',
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboardLayout.component.html',
  styleUrl: './dashboardLayout.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardLayoutComponent implements OnInit {
  public userPermissions: Permiso[] = [];
  user: User | undefined;

  // Menú general
  generalRoutes = [
    { path: 'user', title: 'Usuarios', icon: 'pi pi-users', permission: 'Usuario' },
    { path: 'roles', title: 'Roles', icon: 'pi pi-shield', permission: 'Rol' },
    { path: 'roles-permisos', title: 'Permisos', icon: 'pi pi-key', permission: 'Rol Permiso' },
    { path: 'roles-permisos-usuario', title: 'Asig. Roles', icon: 'pi pi-user-plus', permission: 'Asignacion Roles y Permisos' },
  ];

  // Módulo Inventario
  inventarioRoutes = [
    { path: 'articulo', title: 'Artículos', permission: 'Articulo' },
    { path: 'almacen', title: 'Almacenes', permission: 'Almacen' },
    { path: 'ingreso', title: 'Ingresos', permission: 'Ingreso' },
    { path: 'egreso', title: 'Egresos', permission: 'Egreso' },
    { path: 'traspaso', title: 'Traspasos', permission: 'Traspaso' },
  ];

  // Módulo Ventas
  ventasRoutes = [
    { path: 'customer', title: 'Clientes', permission: 'Cliente' },
    { path: 'sale', title: 'Ventas', permission: 'Venta' },
  ];

  // Módulo Compras (futuro)
  comprasRoutes: any[] = [];

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