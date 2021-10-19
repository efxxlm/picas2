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
import { VerLiberacionSaldosComponent } from './components/ver-liberacion-saldos/ver-liberacion-saldos.component';

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
    path: 'validarRequisitos/:id/validarBalance/:proyectoId',
    component: ValidarBalanceComponent
  },
  {
    path: 'validarRequisitos/:id/verDetalleEditarBalance/:proyectoId',
    component: ValidarBalanceComponent
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
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarBalance/:proyectoId',
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
    path: 'validarRequisitos/:id/validarBalance/:proyectoId/recursos',
    component: RecursosComprometidosPagadosComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarBalance/:proyectoId/recursos',
    component: RecursosComprometidosPagadosComponent
  },
  {
    path: 'validarRequisitos/:id/verDetalleEditarBalance/:proyectoId/recursos',
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
    path: 'validarRequisitos/:id/validarBalance/:proyectoId/ejecucionFinanciera',
    component: VerEjecucionFinancieraComponent
  },
  {
    path: 'validarRequisitos/:id/verDetalleEditarBalance/:proyectoId/ejecucionFinanciera',
    component: VerEjecucionFinancieraComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/validarBalance/:proyectoId/ejecucionFinanciera',
    component: VerEjecucionFinancieraComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarBalance/:proyectoId/ejecucionFinanciera',
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
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarBalance/:proyectoId/traslados',
    component: VerTrasladosRecursosComponent
  },
  {
    path: 'validarRequisitos/:id/validarBalance/:proyectoId/traslados',
    component: VerTrasladosRecursosComponent
  },
  {
    path: 'validarRequisitos/:id/verDetalleEditarBalance/:proyectoId/traslados',
    component: VerTrasladosRecursosComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId/traslados',
    component: VerTrasladosRecursosComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId/traslados/verDetalle/:numeroOrdenGiro',
    component: RegistrarTrasladoGbftrecComponent
  },
  {
    path: 'validarRequisitos/:id/validarBalance/:proyectoId/traslados/verDetalle/:numeroOrdenGiro',
    component: RegistrarTrasladoGbftrecComponent
  },
  {
    path: 'validarRequisitos/:id/verDetalleEditarBalance/:proyectoId/traslados/verDetalle/:numeroOrdenGiro',
    component: RegistrarTrasladoGbftrecComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarBalance/:proyectoId/traslados/verDetalle/:numeroOrdenGiro',
    component: RegistrarTrasladoGbftrecComponent
  },
  /**Liberación */
  {
    path: 'verDetalleEditarRequisitos/:id/validarBalance/:proyectoId/liberacionSaldo',
    component: VerLiberacionSaldosComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarBalance/:proyectoId/liberacionSaldo',
    component: VerLiberacionSaldosComponent
  },
  {
    path: 'validarRequisitos/:id/validarBalance/:proyectoId/liberacionSaldo',
    component: VerLiberacionSaldosComponent
  },
  {
    path: 'validarRequisitos/:id/verDetalleEditarBalance/:proyectoId/liberacionSaldo',
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
export class RegistrarSolicitudLiquidacionContractualRoutingModule { }
