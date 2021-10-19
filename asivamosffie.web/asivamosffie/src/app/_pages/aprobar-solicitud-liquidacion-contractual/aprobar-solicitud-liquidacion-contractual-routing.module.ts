import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { AprobarRequisitosLiquidacionComponent } from './components/aprobar-requisitos-liquidacion/aprobar-requisitos-liquidacion.component';
import { AprobarBalanceComponent } from './components/aprobar-balance/aprobar-balance.component';
import { RecursosComprometidosPagadosComponent } from './components/recursos-comprometidos-pagados/recursos-comprometidos-pagados.component';
import { VerDetalleComponent } from './components/ver-detalle/ver-detalle.component';
import { VerEjecucionFinancieraComponent } from './components/ver-ejecucion-financiera/ver-ejecucion-financiera.component';
import { VerTrasladosRecursosComponent } from './components/ver-traslados-recursos/ver-traslados-recursos.component';
import { DetalleTrasladosComponent } from './components/detalle-traslados/detalle-traslados.component';
import { AprobarInformeFinalComponent } from './components/aprobar-informe-final/aprobar-informe-final.component';
import { RegistrarTrasladoGbftrecComponent } from './components/registrar-traslado-gbftrec/registrar-traslado-gbftrec.component';
import { VerLiberacionSaldosComponent } from './components/ver-liberacion-saldos/ver-liberacion-saldos.component';

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
    path: 'verDetalleEditarRequisitos/:id',
    component: AprobarRequisitosLiquidacionComponent
  },
  {
    path: 'verDetalleRequisitos/:id',
    component: AprobarRequisitosLiquidacionComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/aprobarBalance/:proyectoId',
    component: AprobarBalanceComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/verDetalleEditarBalance/:proyectoId',
    component: AprobarBalanceComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarBalance/:proyectoId',
    component: AprobarBalanceComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/aprobarBalance/:proyectoId',
    component: AprobarBalanceComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId',
    component: AprobarBalanceComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/aprobarBalance/:proyectoId/recursos',
    component: RecursosComprometidosPagadosComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarBalance/:proyectoId/recursos',
    component: RecursosComprometidosPagadosComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/aprobarBalance/:proyectoId/recursos',
    component: RecursosComprometidosPagadosComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/verDetalleEditarBalance/:proyectoId/recursos',
    component: RecursosComprometidosPagadosComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId/recursos',
    component: RecursosComprometidosPagadosComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/aprobarBalance/:proyectoId/recursos/verDetalle/:id',
    component: VerDetalleComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/aprobarBalance/:proyectoId/verEjecucionFinanciera',
    component: VerEjecucionFinancieraComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/aprobarBalance/:proyectoId/verEjecucionFinanciera',
    component: VerEjecucionFinancieraComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/aprobarBalance/:proyectoId/verEjecucionFinanciera',
    component: VerEjecucionFinancieraComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/verDetalleEditarBalance/:proyectoId/verEjecucionFinanciera',
    component: VerEjecucionFinancieraComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId/verEjecucionFinanciera',
    component: VerEjecucionFinancieraComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/aprobarBalance/:proyectoId/verTrasladosRecursos',
    component: VerTrasladosRecursosComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/aprobarBalance/:proyectoId/verTrasladosRecursos',
    component: VerTrasladosRecursosComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarBalance/:proyectoId/verTrasladosRecursos',
    component: VerTrasladosRecursosComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/verDetalleEditarBalance/:proyectoId/verTrasladosRecursos',
    component: VerTrasladosRecursosComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId/verTrasladosRecursos',
    component: VerTrasladosRecursosComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/aprobarBalance/verTrasladosRecursos/verDetalle/:id',
    component: DetalleTrasladosComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/aprobarInformeFinal/:proyectoId',
    component: AprobarInformeFinalComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/aprobarInformeFinal/:proyectoId',
    component: AprobarInformeFinalComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/verDetalleEditarInformeFinal/:proyectoId',
    component: AprobarInformeFinalComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarInformeFinal/:proyectoId',
    component: AprobarInformeFinalComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleInformeFinal/:proyectoId',
    component: AprobarInformeFinalComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId',
    component: AprobarBalanceComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId/verTrasladosRecursos/verDetalle/:numeroOrdenGiro',
    component: RegistrarTrasladoGbftrecComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/aprobarBalance/:proyectoId/verTrasladosRecursos/verDetalle/:numeroOrdenGiro',
    component: RegistrarTrasladoGbftrecComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/verDetalleEditarBalance/:proyectoId/verTrasladosRecursos/verDetalle/:numeroOrdenGiro',
    component: RegistrarTrasladoGbftrecComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarBalance/:proyectoId/verTrasladosRecursos/verDetalle/:numeroOrdenGiro',
    component: RegistrarTrasladoGbftrecComponent
  },
  /**Liberación */
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/aprobarBalance/:proyectoId/liberacionSaldo',
    component: VerLiberacionSaldosComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/aprobarBalance/:proyectoId/liberacionSaldo',
    component: VerLiberacionSaldosComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/verDetalleEditarBalance/:proyectoId/liberacionSaldo',
    component: VerLiberacionSaldosComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId/liberacionSaldo',
    component: VerLiberacionSaldosComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AprobarSolicitudLiquidacionContractualRoutingModule { }
