import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../../environments/environment.development';
import { Product } from '../../../../interfaces/poduct.interface';

const httpOptions = (token: string) => ({
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  })
});

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  apiUrl: string = `${environment.URL_SERVICIOS}/articulo`;

  constructor(private http: HttpClient) { }

  public getProductAll(): Observable<Product[]> {
    const token = sessionStorage.getItem('token');
    if (token) {
      return this.http.get<Product[]>(this.apiUrl, httpOptions(token)).pipe(
        catchError(this.handleError('getProductAll', []))
      );
    } else {
      return of([]);
    }
  }

  public createProduct(product: Product): Observable<Product> {
    const token = sessionStorage.getItem('token');
    if (token) {
      if (!product.nombre || !product.precio || !product.id_categoria) {
        console.error('Faltan datos necesarios para crear el producto');
        return of({} as Product);
      }
      return this.http.post<Product>(this.apiUrl, product, httpOptions(token)).pipe(
        catchError(this.handleError('addProduct', product))
      );
    } else {
      return of({} as Product);
    }
  }

  public deleteProduct(product_id: number): Observable<any> {
    const token = sessionStorage.getItem('token');
    return this.http.delete(`${this.apiUrl}/${product_id}`, httpOptions(token!)).pipe(
      catchError((error) => {
        console.error('Error al eliminar:', error);
        return throwError(() => new Error('Error al eliminar el producto'));
      })
    );
  }

  public updateLimites(id: number, stock: number, stockMin: number, stockMax: number, id_Articulo: number, id_Almacen: number): Observable<any> {
    const token = sessionStorage.getItem('token');
    if (!token) return of(null);

    const body = {
      id: id,
      stock: stock,
      stockMin: stockMin,
      stockMax: stockMax,
      id_Articulo: id_Articulo,
      id_Almacen: id_Almacen
    };

    return this.http.put(
      `${environment.URL_SERVICIOS}/ArticuloAlmacen/${id}`,
      body,
      httpOptions(token)
    ).pipe(
      catchError(this.handleError('updateLimites'))
    );
  }

  public getStockPorArticulo(articuloId: number): Observable<any[]> {
    const token = sessionStorage.getItem('token');
    if (!token) return of([]);
    return this.http.get<any[]>(
      `${environment.URL_SERVICIOS}/ArticuloAlmacen`,
      httpOptions(token)
    ).pipe(
      catchError(this.handleError('getStockPorArticulo', []))
    );
    
  }
  public updateArticulo(id: number, product: any): Observable<any> {
  const token = sessionStorage.getItem('token');
  if (!token) return of(null);
  return this.http.put(
    `${this.apiUrl}/${id}`,
    product,
    httpOptions(token)
  ).pipe(
    catchError(this.handleError('updateArticulo'))
  );
}
  public uploadFoto(articuloId: number, file: File): Observable<any> {
  const token = sessionStorage.getItem('token');
  if (!token) return of(null);

  const formData = new FormData();
  formData.append('file', file);

  return this.http.post(
    `${environment.URL_SERVICIOS}/Articulo/upload/${articuloId}`,
    formData,
    {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
    }
  ).pipe(
    catchError(this.handleError('uploadFoto'))
  );
}

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      return of(result as T);
    };
  }
}