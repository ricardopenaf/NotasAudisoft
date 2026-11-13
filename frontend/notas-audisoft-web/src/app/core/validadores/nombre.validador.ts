import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

const PATRON_SOLO_LETRAS = /^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+(?:\s[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+)*$/;

export function validadorNombre(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const valor = (control.value ?? '').toString().trim();

    if (!valor) {
      return { requerido: true };
    }

    if (valor.length < 2) {
      return { longitudMinima: true };
    }

    if (!PATRON_SOLO_LETRAS.test(valor)) {
      return { patronNombre: true };
    }

    return null;
  };
}
