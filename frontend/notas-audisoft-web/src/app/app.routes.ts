import { Routes } from '@angular/router';
import { EstudiantesComponent } from './features/estudiantes/estudiantes.component';
import { ProfesoresComponent } from './features/profesores/profesores.component';
import { MateriasComponent } from './features/materias/materias.component';
import { NotasComponent } from './features/notas/notas.component';

export const routes: Routes = [
  { path: '', redirectTo: 'estudiantes', pathMatch: 'full' },
  { path: 'estudiantes', component: EstudiantesComponent },
  { path: 'profesores', component: ProfesoresComponent },
  { path: 'materias', component: MateriasComponent },
  { path: 'notas', component: NotasComponent },
  { path: '**', redirectTo: 'estudiantes' }
];
