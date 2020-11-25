import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { ValidarSeguimientoDiarioComponent } from './components/validar-seguimiento-diario/validar-seguimiento-diario.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'validarSeguimiento/:id',
    component: ValidarSeguimientoDiarioComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AprobarSeguimientoDiarioRoutingModule { }
