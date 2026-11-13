import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { NotaService } from '../../core/services/nota.service';
import { EstudianteService } from '../../core/services/estudiante.service';
import { ProfesorService } from '../../core/services/profesor.service';
import { MateriaService } from '../../core/services/materia.service';
import { AlertaService } from '../../core/services/alerta.service';
import { Nota, NotaResumen } from '../../core/models/nota.model';
import { Estudiante } from '../../core/models/estudiante.model';
import { Profesor } from '../../core/models/profesor.model';
import { Materia } from '../../core/models/materia.model';
import { obtenerMensajeError } from '../../core/utilidades/error.util';
import { PaginadorComponent } from '../../shared/paginador/paginador.component';

@Component({
  selector: 'app-notas',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, PaginadorComponent],
  templateUrl: './notas.component.html'
})
export class NotasComponent implements OnInit {
  private readonly notaServicio = inject(NotaService);
  private readonly estudianteServicio = inject(EstudianteService);
  private readonly profesorServicio = inject(ProfesorService);
  private readonly materiaServicio = inject(MateriaService);
  private readonly alerta = inject(AlertaService);
  private readonly fb = inject(FormBuilder);

  readonly resumen = signal<NotaResumen[]>([]);
  readonly paginaActual = signal(1);
  readonly totalPaginas = signal(1);
  readonly tamanoPagina = 5;

  readonly estudiantes = signal<Estudiante[]>([]);
  readonly profesores = signal<Profesor[]>([]);
  readonly materias = signal<Materia[]>([]);

  readonly modoEdicion = signal(false);
  readonly idEnEdicion = signal<number | null>(null);

  readonly filaConsultada = signal<NotaResumen | null>(null);
  readonly notasDeLaFila = signal<Nota[]>([]);

  readonly filaAEliminar = signal<NotaResumen | null>(null);
  readonly notasParaEliminar = signal<Nota[]>([]);
  readonly notaSeleccionadaParaEliminar = signal<number | null>(null);

  readonly formulario = this.fb.nonNullable.group({
    idEstudiante: [0, [Validators.required, Validators.min(1)]],
    idProfesor: [0, [Validators.required, Validators.min(1)]],
    idMateria: [0, [Validators.required, Validators.min(1)]],
    valorNota: [0, [Validators.required, Validators.min(0), Validators.max(5)]]
  });

  ngOnInit(): void {
    this.cargar();
    this.estudianteServicio.obtenerPaginado(1, 100).subscribe((r) => this.estudiantes.set(r.datos));
    this.profesorServicio.obtenerPaginado(1, 100).subscribe((r) => this.profesores.set(r.datos));
    this.materiaServicio.obtenerPaginado(1, 100).subscribe((r) => this.materias.set(r.datos));
  }

  cargar(pagina: number = this.paginaActual()): void {
    this.notaServicio.obtenerResumenPaginado(pagina, this.tamanoPagina).subscribe({
      next: (resultado) => {
        this.resumen.set(resultado.datos);
        this.paginaActual.set(resultado.paginaActual);
        this.totalPaginas.set(resultado.totalPaginas || 1);
      },
      error: () => this.alerta.error('No se pudieron cargar las notas.')
    });
  }

  guardar(): void {
    if (this.formulario.invalid) {
      this.formulario.markAllAsTouched();
      return;
    }

    const valores = this.formulario.getRawValue();

    if (this.modoEdicion() && this.idEnEdicion() !== null) {
      const id = this.idEnEdicion()!;
      this.notaServicio.actualizar(id, { id, ...valores }).subscribe({
        next: () => {
          this.alerta.exito('Nota actualizada correctamente.');
          this.cancelarEdicion();
          this.cargar();
        },
        error: (error) => this.alerta.error(obtenerMensajeError(error, 'No se pudo actualizar la nota.'))
      });
    } else {
      this.notaServicio.crear(valores).subscribe({
        next: () => {
          this.alerta.exito('Nota creada correctamente.');
          this.formulario.reset({ idEstudiante: 0, idProfesor: 0, idMateria: 0, valorNota: 0 });
          this.cargar(1);
        },
        error: (error) => this.alerta.error(obtenerMensajeError(error, 'No se pudo crear la nota.'))
      });
    }
  }

  cancelarEdicion(): void {
    this.modoEdicion.set(false);
    this.idEnEdicion.set(null);
    this.formulario.reset({ idEstudiante: 0, idProfesor: 0, idMateria: 0, valorNota: 0 });
  }

  mostrarNotas(fila: NotaResumen): void {
    this.filaConsultada.set(fila);
    this.notaServicio.obtenerPorEstudianteYMateria(fila.idEstudiante, fila.idMateria).subscribe({
      next: (notas) => this.notasDeLaFila.set(notas),
      error: () => this.alerta.error('No se pudieron cargar las notas de la materia.')
    });
  }

  cerrarMostrarNotas(): void {
    this.filaConsultada.set(null);
    this.notasDeLaFila.set([]);
  }

  editarDesdeModal(nota: Nota): void {
    this.cerrarMostrarNotas();
    this.modoEdicion.set(true);
    this.idEnEdicion.set(nota.id);
    this.formulario.setValue({
      idEstudiante: nota.idEstudiante,
      idProfesor: nota.idProfesor,
      idMateria: nota.idMateria,
      valorNota: nota.valorNota
    });
  }

  abrirEliminar(fila: NotaResumen): void {
    this.filaAEliminar.set(fila);
    this.notaSeleccionadaParaEliminar.set(null);
    this.notaServicio.obtenerPorEstudianteYMateria(fila.idEstudiante, fila.idMateria).subscribe({
      next: (notas) => this.notasParaEliminar.set(notas),
      error: () => this.alerta.error('No se pudieron cargar las notas de la materia.')
    });
  }

  cerrarEliminar(): void {
    this.filaAEliminar.set(null);
    this.notasParaEliminar.set([]);
    this.notaSeleccionadaParaEliminar.set(null);
  }

  eliminarSeleccionada(): void {
    const id = this.notaSeleccionadaParaEliminar();
    if (id === null) {
      this.alerta.advertencia('Seleccione una nota para eliminar.');
      return;
    }

    this.notaServicio.eliminar(id).subscribe({
      next: () => {
        this.alerta.exito('Nota eliminada correctamente.');
        this.cerrarEliminar();
        this.cargar();
      },
      error: (error) => this.alerta.error(obtenerMensajeError(error, 'No se pudo eliminar la nota.'))
    });
  }
}
