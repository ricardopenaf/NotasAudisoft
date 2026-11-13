export interface Estudiante {
  id: number;
  nombreEstudiante: string;
  numeroIdentificacion: number;
}

export interface EstudianteCrear {
  nombreEstudiante: string;
  numeroIdentificacion: number;
}

export interface EstudianteActualizar {
  id: number;
  nombreEstudiante: string;
  numeroIdentificacion: number;
}
