import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { EstudianteService } from '../../core/services/estudiante.service';
import { AlertaService } from '../../core/services/alerta.service';
import { Estudiante } from '../../core/models/estudiante.model';
import { validadorNombre } from '../../core/validadores/nombre.validador';
import { validadorNumeroIdentificacion } from '../../core/validadores/numero-identificacion.validador';
import { obtenerMensajeError } from '../../core/utilidades/error.util';
import { PaginadorComponent } from '../../shared/paginador/paginador.component';

@Component({
  selector: 'app-estudiantes',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, PaginadorComponent],
  templateUrl: './estudiantes.component.html'
})
export class EstudiantesComponent implements OnInit {
  private readonly servicio = inject(EstudianteService);
  private readonly alerta = inject(AlertaService);
  private readonly fb = inject(FormBuilder);

  readonly estudiantes = signal<Estudiante[]>([]);
  readonly paginaActual = signal(1);
  readonly totalPaginas = signal(1);
  readonly tamanoPagina = 5;
  readonly busqueda = signal('');
  readonly cargando = signal(false);
  readonly modoEdicion = signal(false);
  readonly idEnEdicion = signal<number | null>(null);
  readonly estudianteAEliminar = signal<Estudiante | null>(null);

  readonly formulario = this.fb.nonNullable.group({
    nombreEstudiante: ['', [Validators.required, validadorNombre()]],
    numeroIdentificacion: ['', [Validators.required, validadorNumeroIdentificacion()]]
  });

  ngOnInit(): void {
    this.cargar();
  }

  cargar(pagina: number = this.paginaActual()): void {
    this.cargando.set(true);
    this.servicio.obtenerPaginado(pagina, this.tamanoPagina, this.busqueda() || undefined).subscribe({
      next: (resultado) => {
        this.estudiantes.set(resultado.datos);
        this.paginaActual.set(resultado.paginaActual);
        this.totalPaginas.set(resultado.totalPaginas || 1);
        this.cargando.set(false);
      },
      error: () => {
        this.alerta.error('No se pudieron cargar los estudiantes.');
        this.cargando.set(false);
      }
    });
  }

  buscar(evento: Event): void {
    const valor = (evento.target as HTMLInputElement).value;
    this.busqueda.set(valor);
    this.cargar(1);
  }

  editar(estudiante: Estudiante): void {
    this.modoEdicion.set(true);
    this.idEnEdicion.set(estudiante.id);
    this.formulario.setValue({
      nombreEstudiante: estudiante.nombreEstudiante,
      numeroIdentificacion: String(estudiante.numeroIdentificacion)
    });
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

    const valores = this.formulario.getRawValue();
    const nombreEstudiante = valores.nombreEstudiante.trim();
    const numeroIdentificacion = Number(valores.numeroIdentificacion);

    if (this.modoEdicion() && this.idEnEdicion() !== null) {
      const id = this.idEnEdicion()!;
      this.servicio.actualizar(id, { id, nombreEstudiante, numeroIdentificacion }).subscribe({
        next: () => {
          this.alerta.exito('Estudiante actualizado correctamente.');
          this.cancelarEdicion();
          this.cargar();
        },
        error: (error) => this.alerta.error(obtenerMensajeError(error, 'No se pudo actualizar el estudiante.'))
      });
    } else {
      this.servicio.crear({ nombreEstudiante, numeroIdentificacion }).subscribe({
        next: () => {
          this.alerta.exito('Estudiante creado correctamente.');
          this.formulario.reset();
          this.cargar(1);
        },
        error: (error) => this.alerta.error(obtenerMensajeError(error, 'No se pudo crear el estudiante.'))
      });
    }
  }

  confirmarEliminar(estudiante: Estudiante): void {
    this.estudianteAEliminar.set(estudiante);
  }

  cancelarEliminar(): void {
    this.estudianteAEliminar.set(null);
  }

  eliminar(): void {
    const estudiante = this.estudianteAEliminar();
    if (!estudiante) {
      return;
    }

    this.servicio.eliminar(estudiante.id).subscribe({
      next: () => {
        this.alerta.exito('Estudiante eliminado correctamente.');
        this.estudianteAEliminar.set(null);
        this.cargar();
      },
      error: (error) => {
        this.alerta.error(obtenerMensajeError(error, 'No se pudo eliminar el estudiante.'));
        this.estudianteAEliminar.set(null);
      }
    });
  }
}
