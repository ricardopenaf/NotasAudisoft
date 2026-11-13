import { Injectable, signal } from '@angular/core';

export type TipoAlerta = 'success' | 'danger' | 'warning' | 'info';

export interface Alerta {
  id: number;
  tipo: TipoAlerta;
  mensaje: string;
}

@Injectable({ providedIn: 'root' })
export class AlertaService {
  private contador = 0;
  readonly alertas = signal<Alerta[]>([]);

  exito(mensaje: string): void {
    this.mostrar('success', mensaje);
  }

  error(mensaje: string): void {
    this.mostrar('danger', mensaje);
  }

  advertencia(mensaje: string): void {
    this.mostrar('warning', mensaje);
  }

  private mostrar(tipo: TipoAlerta, mensaje: string): void {
    const id = ++this.contador;
    this.alertas.update((lista) => [...lista, { id, tipo, mensaje }]);

    setTimeout(() => this.cerrar(id), 4000);
  }

  cerrar(id: number): void {
    this.alertas.update((lista) => lista.filter((a) => a.id !== id));
  }
}
