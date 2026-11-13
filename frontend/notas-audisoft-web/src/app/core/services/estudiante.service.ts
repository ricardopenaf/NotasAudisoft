import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Estudiante, EstudianteActualizar, EstudianteCrear } from '../models/estudiante.model';
import { ResultadoPaginado } from '../models/resultado-paginado.model';

@Injectable({ providedIn: 'root' })
export class EstudianteService {
  private readonly http = inject(HttpClient);
  private readonly urlBase = `${environment.apiUrl}/estudiantes`;

  obtenerPaginado(pagina: number, tamanoPagina: number, busqueda?: string): Observable<ResultadoPaginado<Estudiante>> {
    let params = new HttpParams().set('pagina', pagina).set('tamanoPagina', tamanoPagina);
    if (busqueda) {
      params = params.set('busqueda', busqueda);
    }
    return this.http.get<ResultadoPaginado<Estudiante>>(this.urlBase, { params });
  }

  obtenerPorId(id: number): Observable<Estudiante> {
    return this.http.get<Estudiante>(`${this.urlBase}/${id}`);
  }

  crear(dto: EstudianteCrear): Observable<Estudiante> {
    return this.http.post<Estudiante>(this.urlBase, dto);
  }

  actualizar(id: number, dto: EstudianteActualizar): Observable<Estudiante> {
    return this.http.put<Estudiante>(`${this.urlBase}/${id}`, dto);
  }

  eliminar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.urlBase}/${id}`);
  }
}
