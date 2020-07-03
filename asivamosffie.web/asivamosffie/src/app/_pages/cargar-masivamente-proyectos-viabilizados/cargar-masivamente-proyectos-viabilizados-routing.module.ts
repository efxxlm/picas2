import { CargarMasivamenteProyectosViabilizadosComponent } from './components/cargar-masivamente-proyectos-viabilizados/cargar-masivamente-proyectos-viabilizados.component';
import { CargarListadoDeProyectosComponent } from './components/cargar-listado-de-proyectos/cargar-listado-de-proyectos.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    component: CargarMasivamenteProyectosViabilizadosComponent
  },
  {
    path: 'cargarProyecto',
    component: CargarListadoDeProyectosComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CargarMasivamenteProyectosViabilizadosRoutingModule { }
