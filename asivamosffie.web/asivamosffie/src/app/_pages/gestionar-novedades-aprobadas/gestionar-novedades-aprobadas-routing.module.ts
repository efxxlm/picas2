import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { RegistrarRevisionJuridicaComponent } from './components/registrar-revision-juridica/registrar-revision-juridica.component';
import { VerDetalleTramiteComponent } from './components/ver-detalle-tramite/ver-detalle-tramite.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'registrarRevisionJuridica/:id',
    component: RegistrarRevisionJuridicaComponent
  },
  {
    path: 'verDetalle/:id',
    component: VerDetalleTramiteComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarNovedadesAprobadasRoutingModule { }
