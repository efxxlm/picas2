import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TituloComponent } from './components/titulo/titulo.component';
import { ExpansionVerificarRequisitosComponent } from './components/expansion-verificar-requisitos/expansion-verificar-requisitos.component'
import { VerDetalleComponent } from './components/ver-detalle/ver-detalle.component';
import { ExpansionGestionarInterventoriaComponent } from './components/expansion-gestionar-interventoria/expansion-gestionar-interventoria.component';

const routes: Routes = [
  {
    path: '',
    component: TituloComponent
  },
  {
    path: 'obraGestionarRequisitos/:id',
    component: ExpansionVerificarRequisitosComponent
  },
  {
    path: 'obraVerDetalle/:id',
    component: VerDetalleComponent
  },
  {
    path: 'interventoriaGestionarRequisitos/:id',
    component: ExpansionGestionarInterventoriaComponent
  },
  {
    path: 'interventoriaVerDetalle/:id',
    component: VerDetalleComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VerificarPreconstruccionRoutingModule { }
