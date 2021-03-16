import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { ValidarRequisitosLiquidacionComponent } from './components/validar-requisitos-liquidacion/validar-requisitos-liquidacion.component';
import { ValidarBalanceComponent } from './components/validar-balance/validar-balance.component';
import { RecursosComprometidosPagadosComponent } from './components/recursos-comprometidos-pagados/recursos-comprometidos-pagados.component';
import { VerDetalleComponent } from './components/ver-detalle/ver-detalle.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'obra/validarRequisitos/:id',
    component: ValidarRequisitosLiquidacionComponent
  },
  {
    path: 'obra/validarRequisitos/:id/validarBalance/:id',
    component: ValidarBalanceComponent
  },
  {
    path: 'obra/validarRequisitos/:id/validarBalance/:id/verRecursos',
    component: RecursosComprometidosPagadosComponent
  },
  {
    path: 'obra/validarRequisitos/:id/validarBalance/:id/verRecursos/verDetalle/1',
    component: VerDetalleComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistrarValidarSolicitudLiquidacionContractualRoutingModule { }
