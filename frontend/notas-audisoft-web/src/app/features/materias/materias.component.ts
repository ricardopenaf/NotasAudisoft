import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MateriaService } from '../../core/services/materia.service';
import { AlertaService } from '../../core/services/alerta.service';
import { Materia } from '../../core/models/materia.model';
import { validadorNombre } from '../../core/validadores/nombre.validador';
import { obtenerMensajeError } from '../../core/utilidades/error.util';
import { PaginadorComponent } from '../../shared/paginador/paginador.component';

@Component({
  selector: 'app-materias',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, PaginadorComponent],
  templateUrl: './materias.component.html'
})
export class MateriasComponent implements OnInit {
  private readonly servicio = inject(MateriaService);
  private readonly alerta = inject(AlertaService);
  private readonly fb = inject(FormBuilder);

  readonly materias = signal<Materia[]>([]);
  readonly paginaActual = signal(1);
  readonly totalPaginas = signal(1);
  readonly tamanoPagina = 5;
  readonly busqueda = signal('');
  readonly modoEdicion = signal(false);
  readonly idEnEdicion = signal<number | null>(null);
  readonly materiaAEliminar = signal<Materia | null>(null);

  readonly formulario = this.fb.nonNullable.group({
    nombreMateria: ['', [Validators.required, validadorNombre()]]
  });

  ngOnInit(): void {
    this.cargar();
  }

  cargar(pagina: number = this.paginaActual()): void {
    this.servicio.obtenerPaginado(pagina, this.tamanoPagina, this.busqueda() || undefined).subscribe({
      next: (resultado) => {
        this.materias.set(resultado.datos);
        this.paginaActual.set(resultado.paginaActual);
        this.totalPaginas.set(resultado.totalPaginas || 1);
      },
      error: () => this.alerta.error('No se pudieron cargar las materias.')
    });
  }

  buscar(evento: Event): void {
    const valor = (evento.target as HTMLInputElement).value;
    this.busqueda.set(valor);
    this.cargar(1);
  }

  editar(materia: Materia): void {
    this.modoEdicion.set(true);
    this.idEnEdicion.set(materia.id);
    this.formulario.setValue({ nombreMateria: materia.nombreMateria });
  }

  cancelarEdicion(): void {
    this.modoEdicion.set(false);
    this.idEnEdicion.set(null);
    this.formulario.reset();
  }

  guardar(): void {
    if (this.formulario.invalid) {
      this.formulario.markAllAsTouched();
      return;
    }

    const nombreMateria = this.formulario.getRawValue().nombreMateria.trim();

    if (this.modoEdicion() && this.idEnEdicion() !== null) {
      const id = this.idEnEdicion()!;
      this.servicio.actualizar(id, { id, nombreMateria }).subscribe({
        next: () => {
          this.alerta.exito('Materia actualizada correctamente.');
          this.cancelarEdicion();
          this.cargar();
        },
        error: (error) => this.alerta.error(obtenerMensajeError(error, 'No se pudo actualizar la materia.'))
      });
    } else {
      this.servicio.crear({ nombreMateria }).subscribe({
        next: () => {
          this.alerta.exito('Materia creada correctamente.');
          this.formulario.reset();
          this.cargar(1);
        },
        error: (error) => this.alerta.error(obtenerMensajeError(error, 'No se pudo crear la materia.'))
      });
    }
  }

  confirmarEliminar(materia: Materia): void {
    this.materiaAEliminar.set(materia);
  }

  cancelarEliminar(): void {
    this.materiaAEliminar.set(null);
  }

  eliminar(): void {
    const materia = this.materiaAEliminar();
    if (!materia) {
      return;
    }

    this.servicio.eliminar(materia.id).subscribe({
      next: () => {
        this.alerta.exito('Materia eliminada correctamente.');
        this.materiaAEliminar.set(null);
        this.cargar();
      },
      error: (error) => {
        this.alerta.error(obtenerMensajeError(error, 'No se pudo eliminar la materia.'));
        this.materiaAEliminar.set(null);
      }
    });
  }
}
