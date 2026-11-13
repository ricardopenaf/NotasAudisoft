import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

const PATRON_SOLO_DIGITOS = /^[0-9]+$/;

export function validadorNumeroIdentificacion(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const valor = (control.value ?? '').toString().trim();

    if (!valor) {
      return { requerido: true };
    }

    if (!PATRON_SOLO_DIGITOS.test(valor)) {
      return { patronNumerico: true };
    }

    if (valor.length < 4 || valor.length > 13) {
      return { longitudInvalida: true };
    }

    return null;
  };
}
