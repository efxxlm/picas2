import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { RegistrarEntregaComponent } from './components/registrar-entrega/registrar-entrega.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'registrarEntrega/:id',
    component: RegistrarEntregaComponent
  },
  // {
  //   path: 'verDetalle/:id',
  //   component: DetalleInformeComponent
  // }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistrarTransferenciaEtcRoutingModule { }
