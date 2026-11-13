export interface Profesor {
  id: number;
  nombreProfesor: string;
  numeroIdentificacion: number;
}

export interface ProfesorCrear {
  nombreProfesor: string;
  numeroIdentificacion: number;
}

export interface ProfesorActualizar {
  id: number;
  nombreProfesor: string;
  numeroIdentificacion: number;
}
