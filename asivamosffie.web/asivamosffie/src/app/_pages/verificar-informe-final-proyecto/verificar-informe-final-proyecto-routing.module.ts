import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { VerificarInformeComponent } from './components/verificar-informe/verificar-informe.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'verificar/:id',
    component: VerificarInformeComponent
  },
  // {
  //   path: 'verDetalle/:id',
  //   component: VerDetalleInformeFinalComponent
  // }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VerificarInformeFinalProyectoRoutingModule { }
