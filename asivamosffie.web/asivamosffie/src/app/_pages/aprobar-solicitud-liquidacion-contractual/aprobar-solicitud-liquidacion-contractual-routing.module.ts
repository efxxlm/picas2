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
    path: 'aprobarRequisitosParaLiquidacion/:id/aprobarBalance',
    component: AprobarBalanceComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/verDetalleEditarBalance',
    component: AprobarBalanceComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarBalance',
    component: AprobarBalanceComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/aprobarBalance',
    component: AprobarBalanceComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance',
    component: AprobarBalanceComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/aprobarBalance/recursos',
    component: RecursosComprometidosPagadosComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/aprobarBalance/recursos/verDetalle/:id',
    component: VerDetalleComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/aprobarBalance/verEjecucionFinanciera',
    component: VerEjecucionFinancieraComponent
  },
  {
    path: 'aprobarRequisitosParaLiquidacion/:id/aprobarBalance/verTrasladosRecursos',
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
    path: 'verDetalleRequisitos/:id/verDetalleBalance',
    component: AprobarBalanceComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AprobarSolicitudLiquidacionContractualRoutingModule { }
