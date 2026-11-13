import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-paginador',
  standalone: true,
  templateUrl: './paginador.component.html'
})
export class PaginadorComponent {
  readonly paginaActual = input.required<number>();
  readonly totalPaginas = input.required<number>();
  readonly cambiarPagina = output<number>();

  irAPagina(pagina: number): void {
    if (pagina < 1 || pagina > this.totalPaginas() || pagina === this.paginaActual()) {
      return;
    }
    this.cambiarPagina.emit(pagina);
  }

  get paginas(): number[] {
    return Array.from({ length: this.totalPaginas() }, (_, i) => i + 1);
  }
}
