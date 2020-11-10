import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TablaRegistrarRequisitosComponent } from './components/tabla-registrar-requisitos/tabla-registrar-requisitos.component';
import { ExpansionGestionarRequisitosComponent } from './components/expansion-gestionar-requisitos/expansion-gestionar-requisitos.component';

const routes: Routes = [
  {
    path: '',
    component: TablaRegistrarRequisitosComponent
  },
  {
    path: 'gestionarRequisitos/:id',
    component: ExpansionGestionarRequisitosComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FasePreconstruccionRoutingModule { }
