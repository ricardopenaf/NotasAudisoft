import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Nota, NotaActualizar, NotaCrear, NotaResumen } from '../models/nota.model';
import { ResultadoPaginado } from '../models/resultado-paginado.model';

@Injectable({ providedIn: 'root' })
export class NotaService {
  private readonly http = inject(HttpClient);
  private readonly urlBase = `${environment.apiUrl}/notas`;

  obtenerResumenPaginado(pagina: number, tamanoPagina: number): Observable<ResultadoPaginado<NotaResumen>> {
    const params = new HttpParams().set('pagina', pagina).set('tamanoPagina', tamanoPagina);
    return this.http.get<ResultadoPaginado<NotaResumen>>(`${this.urlBase}/resumen`, { params });
  }

  obtenerPorEstudianteYMateria(idEstudiante: number, idMateria: number): Observable<Nota[]> {
    return this.http.get<Nota[]>(`${this.urlBase}/estudiante/${idEstudiante}/materia/${idMateria}`);
  }

  obtenerPorId(id: number): Observable<Nota> {
    return this.http.get<Nota>(`${this.urlBase}/${id}`);
  }

  crear(dto: NotaCrear): Observable<Nota> {
    return this.http.post<Nota>(this.urlBase, dto);
  }

  actualizar(id: number, dto: NotaActualizar): Observable<Nota> {
    return this.http.put<Nota>(`${this.urlBase}/${id}`, dto);
  }

  eliminar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.urlBase}/${id}`);
  }
}
