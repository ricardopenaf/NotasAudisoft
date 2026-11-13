import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AlertaComponent } from './shared/alerta/alerta.component';

@Component({
  imports: [RouterOutlet, RouterLink, RouterLinkActive, AlertaComponent],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {}
