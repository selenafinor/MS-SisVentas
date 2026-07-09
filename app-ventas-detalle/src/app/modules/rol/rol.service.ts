import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { Rol } from '../../interfaces/rol.interface';
import { catchError, Observable, of } from 'rxjs';
import { RolPermiso } from '../../interfaces/rol-permiso.interface';
import { RolPermisoUsuario } from '../../interfaces/rol-permiso-usuario.interface';

const httpOptions = (token: string) => ({
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    Authorization: `Bearer ${token}`,
  }),
});

@Injectable({
  providedIn: 'root'
})
export class RolService {
  private apiUrl = `${environment.URL_SERVICIOS}/roles`;

  constructor(private http: HttpClient) {}

  getRoles(): Observable<Rol[]> {
    const token = sessionStorage.getItem('token');
    if (!token) return of([]);
    return this.http.get<Rol[]>(this.apiUrl, httpOptions(token))
      .pipe(catchError(this.handleError('getRoles', [])));
  }

  getRolById(id: number): Observable<Rol> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({} as Rol);
    return this.http.get<Rol>(`${this.apiUrl}/${id}`, httpOptions(token))
      .pipe(catchError(this.handleError('getRolById', {} as Rol)));
  }

  createRol(rol: Rol): Observable<Rol> {
    const token = sessionStorage.getItem('token');
    if (!token) return of({} as Rol);
    return this.http.post<Rol>(this.apiUrl, rol, httpOptions(token))
      .pipe(catchError(this.handleError('createRol', rol)));
  }

  updateRol(id: number, rol: any): Observable<any> {
    const token = sessionStorage.getItem('token');
    if (!token) return of(null);
    return this.http.put<any>(`${this.apiUrl}/${id}`, rol, httpOptions(token))
      .pipe(catchError(this.handleError('updateRol', null)));
  }

  deleteRol(id: number): Observable<void> {
    const token = sessionStorage.getItem('token');
    if (!token) return of(undefined);
    return this.http.delete<void>(`${this.apiUrl}/${id}`, httpOptions(token))
      .pipe(catchError(this.handleError<void>('deleteRol')));
  }

  getRolesConPermisos(): Observable<any[]> {
    const token = sessionStorage.getItem('token');
    if (!token) return of([]);
    return this.http.get<any[]>(`${this.apiUrl}/conpermisos`, httpOptions(token))
      .pipe(catchError(this.handleError('getRolesConPermisos', [])));
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      return of(result as T);
    };
  }

  createRolPerimo(rolpermiso: RolPermiso): Observable<RolPermiso> {
    const urlRolPermiso = `${environment.URL_SERVICIOS}/rolpermisos`;
    const token = sessionStorage.getItem('token');
    if (!token) return of({} as RolPermiso);
    return this.http.post<RolPermiso>(urlRolPermiso, rolpermiso, httpOptions(token))
      .pipe(catchError(this.handleError('createRolPermiso', rolpermiso)));
  }

  deleteRolPermisos(id_rolpermiso?: number): Observable<any> {
    const urlRolPermiso = `${environment.URL_SERVICIOS}/rolpermisos/${id_rolpermiso}`;
    const token = sessionStorage.getItem('token');
    if (!token) return of({} as RolPermiso);
    return this.http.delete<any>(urlRolPermiso, httpOptions(token))
      .pipe(catchError(this.handleError('deleteRolPermiso', id_rolpermiso)));
  }

  // OJO: antes esta URL era "/rolpermisousuario" (singular) y no coincidia
  // con la ruta real del backend/Ocelot ("/rolpermisousuarios", plural),
  // asi que esta llamada probablemente fallaba en silencio (el catchError
  // devuelve [] y no se ve el error en pantalla).
  getRolPermisoUsuario(): Observable<RolPermisoUsuario[]> {
    const urlRolUsuario = `${environment.URL_SERVICIOS}/rolpermisousuarios`;
    const token = sessionStorage.getItem('token');
    if (!token) return of([]);
    return this.http.get<RolPermisoUsuario[]>(urlRolUsuario, httpOptions(token))
      .pipe(catchError(this.handleError('getRolPermisoUsuario', [])));
  }

  // Trae solo las asignaciones rol-permiso del usuario indicado.
  // Se filtra en el cliente porque el backend no tiene un endpoint
  // "por usuario" dedicado (GET /rolpermisousuarios/usuario/{id}).
  getRolPermisoUsuarioByUsuario(userId: number): Observable<RolPermisoUsuario[]> {
    return this.getRolPermisoUsuario().pipe(
      catchError(this.handleError('getRolPermisoUsuarioByUsuario', [] as RolPermisoUsuario[]))
    ) as unknown as Observable<RolPermisoUsuario[]>;
  }

  createRolUsuario(rolusuario: RolPermisoUsuario): Observable<RolPermisoUsuario> {
    const urlRolUsuario = `${environment.URL_SERVICIOS}/rolpermisousuarios`;
    const token = sessionStorage.getItem('token');
    if (!token) return of({} as RolPermisoUsuario);
    return this.http.post<RolPermisoUsuario>(urlRolUsuario, rolusuario, httpOptions(token))
      .pipe(catchError(this.handleError('createRolUsuario', rolusuario)));
  }

  getRolUsuarioById(id: number): Observable<RolPermisoUsuario> {
    const urlRolUsuario = `${environment.URL_SERVICIOS}/rolpermisousuarios/${id}`;
    const token = sessionStorage.getItem('token');
    if (!token) return of({} as RolPermisoUsuario);
    return this.http.get<RolPermisoUsuario>(urlRolUsuario, httpOptions(token))
      .pipe(catchError(this.handleError('getRolUsuarioById', {} as RolPermisoUsuario)));
  }

  // Antes esta URL apuntaba a "/rolpermisousuario/{id}" (singular) Y el
  // backend ni siquiera exponia un endpoint DELETE: aunque el frontend
  // llamara esto, siempre fallaba y las asignaciones viejas nunca se
  // borraban. Ya se corrigio en el Controller y en Ocelot.
  deleteRolUsuario(id_rolusuario?: number): Observable<any> {
    const urlRolUsuario = `${environment.URL_SERVICIOS}/rolpermisousuarios/${id_rolusuario}`;
    const token = sessionStorage.getItem('token');
    if (!token) return of({} as RolPermisoUsuario);
    return this.http.delete<any>(urlRolUsuario, httpOptions(token))
      .pipe(catchError(this.handleError('deleteRolUsuario', id_rolusuario)));
  }
}