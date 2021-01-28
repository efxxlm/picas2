import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { VerificarSeguimientoComponent } from './components/verificar-seguimiento/verificar-seguimiento.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'verificarSeguimiento/:id',
    component: VerificarSeguimientoComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VerificarSeguimientoDiarioRoutingModule { }
