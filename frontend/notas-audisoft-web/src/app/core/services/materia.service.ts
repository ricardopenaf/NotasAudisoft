import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Materia, MateriaActualizar, MateriaCrear } from '../models/materia.model';
import { ResultadoPaginado } from '../models/resultado-paginado.model';

@Injectable({ providedIn: 'root' })
export class MateriaService {
  private readonly http = inject(HttpClient);
  private readonly urlBase = `${environment.apiUrl}/materias`;

  obtenerPaginado(pagina: number, tamanoPagina: number, busqueda?: string): Observable<ResultadoPaginado<Materia>> {
    let params = new HttpParams().set('pagina', pagina).set('tamanoPagina', tamanoPagina);
    if (busqueda) {
      params = params.set('busqueda', busqueda);
    }
    return this.http.get<ResultadoPaginado<Materia>>(this.urlBase, { params });
  }

  obtenerPorId(id: number): Observable<Materia> {
    return this.http.get<Materia>(`${this.urlBase}/${id}`);
  }

  crear(dto: MateriaCrear): Observable<Materia> {
    return this.http.post<Materia>(this.urlBase, dto);
  }

  actualizar(id: number, dto: MateriaActualizar): Observable<Materia> {
    return this.http.put<Materia>(`${this.urlBase}/${id}`, dto);
  }

  eliminar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.urlBase}/${id}`);
  }
}
