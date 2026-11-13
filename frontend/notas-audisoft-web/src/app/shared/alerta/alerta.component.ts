import { Component, inject } from '@angular/core';
import { AlertaService } from '../../core/services/alerta.service';

@Component({
  selector: 'app-alerta',
  standalone: true,
  templateUrl: './alerta.component.html',
  styleUrl: './alerta.component.css'
})
export class AlertaComponent {
  protected readonly servicioAlerta = inject(AlertaService);
}
