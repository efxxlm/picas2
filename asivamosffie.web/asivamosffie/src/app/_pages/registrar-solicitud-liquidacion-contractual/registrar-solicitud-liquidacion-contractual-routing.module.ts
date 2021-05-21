import { RegistrarTrasladoGbftrecComponent } from './components/registrar-traslado-gbftrec/registrar-traslado-gbftrec.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { ValidarRequerimientosLiquidacionesComponent } from './components/validar-requerimientos-liquidaciones/validar-requerimientos-liquidaciones.component';
import { ValidarBalanceComponent } from './components/validar-balance/validar-balance.component';
import { RecursosComprometidosPagadosComponent } from './components/recursos-comprometidos-pagados/recursos-comprometidos-pagados.component';
import { ValidarInformeFinalComponent } from './components/validar-informe-final/validar-informe-final.component';
import { VerEjecucionFinancieraComponent } from './components/ver-ejecucion-financiera/ver-ejecucion-financiera.component';
import { VerTrasladosRecursosComponent } from './components/ver-traslados-recursos/ver-traslados-recursos.component';
import { DetalleTrasladosComponent } from './components/detalle-traslados/detalle-traslados.component';

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
    path: 'verDetalleEditarRequisitos/:id',
    component:ValidarRequerimientosLiquidacionesComponent
  },
  {
    path: 'verDetalleRequisitos/:id',
    component: ValidarRequerimientosLiquidacionesComponent
  },
  {
    path: 'validarRequisitos/:id/validarBalance/:id',
    component: ValidarBalanceComponent
  },
  {
    path: 'validarRequisitos/:id/validarBalance/recursos',
    component: RecursosComprometidosPagadosComponent
  },
  {
    path: 'validarRequisitos/:id/validarInformeFinal/:proyectoId',
    component: ValidarInformeFinalComponent
  },
  {
    path: 'validarRequisitos/:id/verDetalleEditarInformeFinal/:proyectoId',
    component: ValidarInformeFinalComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarInformeFinal/:proyectoId',
    component: ValidarInformeFinalComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleInformeFinal/:proyectoId',
    component: ValidarInformeFinalComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/validarInformeFinal/:proyectoId',
    component: ValidarInformeFinalComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/validarBalance/:proyectoId',
    component: ValidarBalanceComponent
  },
  {
    path: 'validarRequisitos/:id/validarBalance/:proyectoId',
    component: ValidarBalanceComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarValidarBalance/:proyectoId',
    component: ValidarBalanceComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleValidarBalance/:proyectoId',
    component: ValidarBalanceComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId',
    component: ValidarBalanceComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarValidarBalance/:proyectoId/recursos',
    component: RecursosComprometidosPagadosComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleValidarBalance/:proyectoId/recursos',
    component: RecursosComprometidosPagadosComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId/recursos',
    component: RecursosComprometidosPagadosComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/validarBalance/:proyectoId/recursos',
    component: RecursosComprometidosPagadosComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/validarBalance/:proyectoId/ejecucionFinanciera',
    component: VerEjecucionFinancieraComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleBalance/:proyectoId/ejecucionFinanciera',
    component: VerEjecucionFinancieraComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId/ejecucionFinanciera',
    component: VerEjecucionFinancieraComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/validarBalance/:proyectoId/traslados',
    component: VerTrasladosRecursosComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId/traslados',
    component: VerTrasladosRecursosComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId/traslados/verDetalle/:numeroOrdenGiro',
    component: RegistrarTrasladoGbftrecComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistrarSolicitudLiquidacionContractualRoutingModule { }
