import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Profesor, ProfesorActualizar, ProfesorCrear } from '../models/profesor.model';
import { ResultadoPaginado } from '../models/resultado-paginado.model';

@Injectable({ providedIn: 'root' })
export class ProfesorService {
  private readonly http = inject(HttpClient);
  private readonly urlBase = `${environment.apiUrl}/profesores`;

  obtenerPaginado(pagina: number, tamanoPagina: number, busqueda?: string): Observable<ResultadoPaginado<Profesor>> {
    let params = new HttpParams().set('pagina', pagina).set('tamanoPagina', tamanoPagina);
    if (busqueda) {
      params = params.set('busqueda', busqueda);
    }
    return this.http.get<ResultadoPaginado<Profesor>>(this.urlBase, { params });
  }

  obtenerPorId(id: number): Observable<Profesor> {
    return this.http.get<Profesor>(`${this.urlBase}/${id}`);
  }

  crear(dto: ProfesorCrear): Observable<Profesor> {
    return this.http.post<Profesor>(this.urlBase, dto);
  }

  actualizar(id: number, dto: ProfesorActualizar): Observable<Profesor> {
    return this.http.put<Profesor>(`${this.urlBase}/${id}`, dto);
  }

  eliminar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.urlBase}/${id}`);
  }
}
