import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { AprobarRequisitosLiquidacionComponent } from './components/aprobar-requisitos-liquidacion/aprobar-requisitos-liquidacion.component';
import { AprobarBalanceComponent } from './components/aprobar-balance/aprobar-balance.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id',
    component: AprobarRequisitosLiquidacionComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/aprobarBalance',
    component: AprobarBalanceComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AprobarSolicitudLiquidacionContractualRoutingModule { }
