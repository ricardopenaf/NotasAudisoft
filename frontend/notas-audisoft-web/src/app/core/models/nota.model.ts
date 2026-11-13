export interface Nota {
  id: number;
  idEstudiante: number;
  nombreEstudiante: string;
  idProfesor: number;
  nombreProfesor: string;
  idMateria: number;
  nombreMateria: string;
  valorNota: number;
}

export interface NotaCrear {
  idEstudiante: number;
  idProfesor: number;
  idMateria: number;
  valorNota: number;
}

export interface NotaActualizar extends NotaCrear {
  id: number;
}

export interface NotaResumen {
  idEstudiante: number;
  nombreEstudiante: string;
  idMateria: number;
  nombreMateria: string;
  idProfesor: number;
  nombreProfesor: string;
  promedioNota: number;
  cantidadNotas: number;
}
