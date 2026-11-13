export interface ResultadoPaginado<T> {
  datos: T[];
  paginaActual: number;
  tamanoPagina: number;
  totalRegistros: number;
  totalPaginas: number;
}
