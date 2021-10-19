import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { CurrencyMaskInputMode, NgxCurrencyModule } from "ngx-currency";
import { DetalleBalanceFinancGtlcComponent } from "./components/detalle-balance-financ-gtlc/detalle-balance-financ-gtlc.component";
import { DetalleInformeFinalGtlcComponent } from "./components/detalle-informe-final-gtlc/detalle-informe-final-gtlc.component";
import { DetalleOgGtlcComponent } from "./components/detalle-og-gtlc/detalle-og-gtlc.component";
import { DetalleTrasladoGtlcComponent } from "./components/detalle-traslado-gtlc/detalle-traslado-gtlc.component";
import { EjecucionFinancieraGtlcComponent } from "./components/ejecucion-financiera-gtlc/ejecucion-financiera-gtlc.component";
import { GestionarTramiteLiqContractualComponent } from "./components/gestionar-tramite-liq-contractual/gestionar-tramite-liq-contractual.component";
import { RecursosComproPagadosGtlcComponent } from "./components/recursos-compro-pagados-gtlc/recursos-compro-pagados-gtlc.component";
import { RegistrarTrasladoGbftrecComponent } from "./components/registrar-traslado-gbftrec/registrar-traslado-gbftrec.component";
import { TrasladoRecursosGtlcComponent } from "./components/traslado-recursos-gtlc/traslado-recursos-gtlc.component";
import { VerDetalleEditarVerificacionComponent } from "./components/ver-detalle-editar-verificacion/ver-detalle-editar-verificacion.component";
import { VerDetalleVerificacionGtlcComponent } from "./components/ver-detalle-verificacion-gtlc/ver-detalle-verificacion-gtlc.component";
import { VerLiberacionSaldosComponent } from "./components/ver-liberacion-saldos/ver-liberacion-saldos.component";
import { VerificarBalanceGtlcComponent } from "./components/verificar-balance-gtlc/verificar-balance-gtlc.component";
import { VerificarInformeGtlcComponent } from "./components/verificar-informe-gtlc/verificar-informe-gtlc.component";
import { VerificarRequisitosGtlcComponent } from "./components/verificar-requisitos-gtlc/verificar-requisitos-gtlc.component";

export const customCurrencyMaskConfig = {
  align: "right",
  allowNegative: true,
  allowZero: true,
  decimal: ",",
  precision: 0,
  prefix: "$ ",
  suffix: "",
  thousands: ".",
  nullable: true,
  min: null,
  max: null,
  inputMode: CurrencyMaskInputMode.FINANCIAL
};
const routes: Routes = [
  {
    path: '',
    component: GestionarTramiteLiqContractualComponent
  },
  {
    path : 'verificarRequisitos/:id',
    component: VerificarRequisitosGtlcComponent
  },
  {
    path : 'verDetalleEditarRequisitos/:id',
    component: VerificarRequisitosGtlcComponent
  },
  {
    path : 'verDetalleRequisitos/:id',
    component: VerificarRequisitosGtlcComponent
  },
  {
    path: 'verificarRequisitos/:id/verificarBalance',
    component: VerificarBalanceGtlcComponent
  },
  {
    path: 'detalleOrdengiro/:id',
    component: DetalleOgGtlcComponent
  },
  {
    path: 'trasladoRecursos/:id',
    component: TrasladoRecursosGtlcComponent
  },
  {
    path: 'detalleTraslado/:id',
    component: DetalleTrasladoGtlcComponent
  },
  {
    path: 'verDetalleEditarVerificacion/:id',
    component: VerDetalleEditarVerificacionComponent
  },
  {
    path: 'verDetalleVerificacion/:id',
    component: VerDetalleVerificacionGtlcComponent
  },
  {
    path: 'detalleBalanceFinanciero/:id',
    component: DetalleBalanceFinancGtlcComponent
  },
  {
    path: 'detalleInformeFinal/:id',
    component: DetalleInformeFinalGtlcComponent
  },
  {
    path: 'verificarRequisitos/:id/verificarInformeFinal/:proyectoId',
    component: VerificarInformeGtlcComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verificarInformeFinal/:proyectoId',
    component: VerificarInformeGtlcComponent
  },
  {
    path: 'verificarRequisitos/:id/verDetalleEditarInformeFinal/:proyectoId',
    component: VerificarInformeGtlcComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarInformeFinal/:proyectoId',
    component: VerificarInformeGtlcComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleInformeFinal/:proyectoId',
    component: VerificarInformeGtlcComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verificarBalance/:proyectoId',
    component: VerificarBalanceGtlcComponent
  },
  {
    path: 'verificarRequisitos/:id/verificarBalance/:proyectoId',
    component: VerificarBalanceGtlcComponent
  },
  {
    path: 'verificarRequisitos/:id/verDetalleEditarBalance/:proyectoId',
    component: VerificarBalanceGtlcComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId',
    component: VerificarBalanceGtlcComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarBalance/:proyectoId',
    component: VerificarBalanceGtlcComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId',
    component: VerificarBalanceGtlcComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId/recursosComprometidos',
    component: RecursosComproPagadosGtlcComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verificarBalance/:proyectoId/recursosComprometidos',
    component: RecursosComproPagadosGtlcComponent
  },
  {
    path: 'verificarRequisitos/:id/verDetalleBalance/:proyectoId/recursosComprometidos',
    component: RecursosComproPagadosGtlcComponent
  },
  {
    path: 'verificarRequisitos/:id/verDetalleEditarBalance/:proyectoId/recursosComprometidos',
    component: RecursosComproPagadosGtlcComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarBalance/:proyectoId/recursosComprometidos',
    component: RecursosComproPagadosGtlcComponent
  },
  {
    path: 'verificarRequisitos/:id/verificarBalance/:proyectoId/recursosComprometidos',
    component: RecursosComproPagadosGtlcComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId/ejecucionFinanciera',
    component: EjecucionFinancieraGtlcComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verificarBalance/:proyectoId/ejecucionFinanciera',
    component: EjecucionFinancieraGtlcComponent
  },
  {
    path: 'verificarRequisitos/:id/verDetalleEditarBalance/:proyectoId/ejecucionFinanciera',
    component: EjecucionFinancieraGtlcComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarBalance/:proyectoId/ejecucionFinanciera',
    component: EjecucionFinancieraGtlcComponent
  },
  {
    path: 'verificarRequisitos/:id/verDetalleBalance/:proyectoId/ejecucionFinanciera',
    component: EjecucionFinancieraGtlcComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleBalance/:proyectoId/ejecucionFinanciera',
    component: EjecucionFinancieraGtlcComponent
  },
  {
    path: 'verificarRequisitos/:id/verificarBalance/:proyectoId/ejecucionFinanciera',
    component: EjecucionFinancieraGtlcComponent
  },
  {
    path: 'verificarRequisitos/:id/verificarBalance/:proyectoId/trasladoRecursos',
    component: TrasladoRecursosGtlcComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verificarBalance/:proyectoId/trasladoRecursos',
    component: TrasladoRecursosGtlcComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarBalance/:proyectoId/trasladoRecursos',
    component: TrasladoRecursosGtlcComponent
  },
  {
    path: 'verificarRequisitos/:id/verDetalleEditarBalance/:proyectoId/trasladoRecursos',
    component: TrasladoRecursosGtlcComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId/trasladoRecursos',
    component: TrasladoRecursosGtlcComponent
  },
  {
    path: 'verDetalle/:id/verDetalleBalance/:proyectoId/trasladoRecursos/verDetalle/:id',
    component: TrasladoRecursosGtlcComponent
  },
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId/trasladoRecursos/verDetalle/:numeroOrdenGiro',
    component: RegistrarTrasladoGbftrecComponent
  },
  {
    path: 'verificarRequisitos/:id/verificarBalance/:proyectoId/trasladoRecursos/verDetalle/:numeroOrdenGiro',
    component: RegistrarTrasladoGbftrecComponent
  },
  {
    path: 'verificarRequisitos/:id/verDetalleEditarBalance/:proyectoId/trasladoRecursos/verDetalle/:numeroOrdenGiro',
    component: RegistrarTrasladoGbftrecComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarBalance/:proyectoId/trasladoRecursos/verDetalle/:numeroOrdenGiro',
    component: RegistrarTrasladoGbftrecComponent
  },
  /**Liberación */
  {
    path: 'verDetalleRequisitos/:id/verDetalleBalance/:proyectoId/liberacionSaldo',
    component: VerLiberacionSaldosComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verificarBalance/:proyectoId/liberacionSaldo',
    component: VerLiberacionSaldosComponent
  },
  {
    path: 'verificarRequisitos/:id/verDetalleEditarBalance/:proyectoId/liberacionSaldo',
    component: VerLiberacionSaldosComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleEditarBalance/:proyectoId/liberacionSaldo',
    component: VerLiberacionSaldosComponent
  },
  {
    path: 'verificarRequisitos/:id/verDetalleBalance/:proyectoId/liberacionSaldo',
    component: VerLiberacionSaldosComponent
  },
  {
    path: 'verDetalleEditarRequisitos/:id/verDetalleBalance/:proyectoId/liberacionSaldo',
    component: VerLiberacionSaldosComponent
  },
  {
    path: 'verificarRequisitos/:id/verificarBalance/:proyectoId/liberacionSaldo',
    component: VerLiberacionSaldosComponent
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes),NgxCurrencyModule.forRoot(customCurrencyMaskConfig)],
  exports: [RouterModule]
})
export class GestionarTramiteLiqContractualRoutingModule { }
