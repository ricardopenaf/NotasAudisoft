import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProfesorService } from '../../core/services/profesor.service';
import { AlertaService } from '../../core/services/alerta.service';
import { Profesor } from '../../core/models/profesor.model';
import { validadorNombre } from '../../core/validadores/nombre.validador';
import { validadorNumeroIdentificacion } from '../../core/validadores/numero-identificacion.validador';
import { obtenerMensajeError } from '../../core/utilidades/error.util';
import { PaginadorComponent } from '../../shared/paginador/paginador.component';

@Component({
  selector: 'app-profesores',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, PaginadorComponent],
  templateUrl: './profesores.component.html'
})
export class ProfesoresComponent implements OnInit {
  private readonly servicio = inject(ProfesorService);
  private readonly alerta = inject(AlertaService);
  private readonly fb = inject(FormBuilder);

  readonly profesores = signal<Profesor[]>([]);
  readonly paginaActual = signal(1);
  readonly totalPaginas = signal(1);
  readonly tamanoPagina = 5;
  readonly busqueda = signal('');
  readonly modoEdicion = signal(false);
  readonly idEnEdicion = signal<number | null>(null);
  readonly profesorAEliminar = signal<Profesor | null>(null);

  readonly formulario = this.fb.nonNullable.group({
    nombreProfesor: ['', [Validators.required, validadorNombre()]],
    numeroIdentificacion: ['', [Validators.required, validadorNumeroIdentificacion()]]
  });

  ngOnInit(): void {
    this.cargar();
  }

  cargar(pagina: number = this.paginaActual()): void {
    this.servicio.obtenerPaginado(pagina, this.tamanoPagina, this.busqueda() || undefined).subscribe({
      next: (resultado) => {
        this.profesores.set(resultado.datos);
        this.paginaActual.set(resultado.paginaActual);
        this.totalPaginas.set(resultado.totalPaginas || 1);
      },
      error: () => this.alerta.error('No se pudieron cargar los profesores.')
    });
  }

  buscar(evento: Event): void {
    const valor = (evento.target as HTMLInputElement).value;
    this.busqueda.set(valor);
    this.cargar(1);
  }

  editar(profesor: Profesor): void {
    this.modoEdicion.set(true);
    this.idEnEdicion.set(profesor.id);
    this.formulario.setValue({
      nombreProfesor: profesor.nombreProfesor,
      numeroIdentificacion: String(profesor.numeroIdentificacion)
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
    const nombreProfesor = valores.nombreProfesor.trim();
    const numeroIdentificacion = Number(valores.numeroIdentificacion);

    if (this.modoEdicion() && this.idEnEdicion() !== null) {
      const id = this.idEnEdicion()!;
      this.servicio.actualizar(id, { id, nombreProfesor, numeroIdentificacion }).subscribe({
        next: () => {
          this.alerta.exito('Profesor actualizado correctamente.');
          this.cancelarEdicion();
          this.cargar();
        },
        error: (error) => this.alerta.error(obtenerMensajeError(error, 'No se pudo actualizar el profesor.'))
      });
    } else {
      this.servicio.crear({ nombreProfesor, numeroIdentificacion }).subscribe({
        next: () => {
          this.alerta.exito('Profesor creado correctamente.');
          this.formulario.reset();
          this.cargar(1);
        },
        error: (error) => this.alerta.error(obtenerMensajeError(error, 'No se pudo crear el profesor.'))
      });
    }
  }

  confirmarEliminar(profesor: Profesor): void {
    this.profesorAEliminar.set(profesor);
  }

  cancelarEliminar(): void {
    this.profesorAEliminar.set(null);
  }

  eliminar(): void {
    const profesor = this.profesorAEliminar();
    if (!profesor) {
      return;
    }

    this.servicio.eliminar(profesor.id).subscribe({
      next: () => {
        this.alerta.exito('Profesor eliminado correctamente.');
        this.profesorAEliminar.set(null);
        this.cargar();
      },
      error: (error) => {
        this.alerta.error(obtenerMensajeError(error, 'No se pudo eliminar el profesor.'));
        this.profesorAEliminar.set(null);
      }
    });
  }
}
