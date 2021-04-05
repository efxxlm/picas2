import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { ValidarRequerimientosLiquidacionesComponent } from './components/validar-requerimientos-liquidaciones/validar-requerimientos-liquidaciones.component';
import { ValidarBalanceComponent } from './components/validar-balance/validar-balance.component';
import { RecursosComprometidosPagadosComponent } from './components/recursos-comprometidos-pagados/recursos-comprometidos-pagados.component';
import { ValidarInformeFinalComponent } from './components/validar-informe-final/validar-informe-final.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'validarRequisitos/:id',
    component: ValidarRequerimientosLiquidacionesComponent
  },
  {
    path: 'validarRequisitos/:id/validarBalance',
    component: ValidarBalanceComponent
  },
  {
    path: 'validarRequisitos/:id/validarBalance/recursos',
    component: RecursosComprometidosPagadosComponent
  },
  {
    path: 'validarRequisitos/:id/validarInformeFinal/:proyectoId',
    component: ValidarInformeFinalComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistrarSolicitudLiquidacionContractualRoutingModule { }
