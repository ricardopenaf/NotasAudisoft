import { HttpErrorResponse } from '@angular/common/http';

export function obtenerMensajeError(error: unknown, mensajePorDefecto: string): string {
  if (error instanceof HttpErrorResponse) {
    const cuerpo = error.error as { mensaje?: string } | null;
    if (cuerpo?.mensaje) {
      return cuerpo.mensaje;
    }
  }
  return mensajePorDefecto;
}
