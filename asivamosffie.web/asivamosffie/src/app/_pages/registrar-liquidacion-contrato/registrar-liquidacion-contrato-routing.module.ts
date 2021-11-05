import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CurrencyMaskInputMode, NgxCurrencyModule } from "ngx-currency";
import { DetalleBalanceFinancieroRlcComponent } from "./components/detalle-balance-financiero-rlc/detalle-balance-financiero-rlc.component";
import { DetalleInformeFinalRlcComponent } from "./components/detalle-informe-final-rlc/detalle-informe-final-rlc.component";
import { DetalleOgRlcComponent } from "./components/detalle-og-rlc/detalle-og-rlc.component";
import { DetalleTrasladoRlcComponent } from "./components/detalle-traslado-rlc/detalle-traslado-rlc.component";
import { EjecucionFinancieraRlcComponent } from "./components/ejecucion-financiera-rlc/ejecucion-financiera-rlc.component";
import { GestionarSolicitudRlcComponent } from "./components/gestionar-solicitud-rlc/gestionar-solicitud-rlc.component";
import { RecursosComproPagadosRlcComponent } from "./components/recursos-compro-pagados-rlc/recursos-compro-pagados-rlc.component";
import { RegistrarLiquidacionContratoComponent } from "./components/registrar-liquidacion-contrato/registrar-liquidacion-contrato.component";
import { TrasladoRecursosRlcComponent } from "./components/traslado-recursos-rlc/traslado-recursos-rlc.component";
import { VerLiberacionSaldosComponent } from "./components/ver-liberacion-saldos/ver-liberacion-saldos.component";

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
    component: RegistrarLiquidacionContratoComponent
  },
  {
    path: 'gestionarSolicitud/:id',
    component: GestionarSolicitudRlcComponent
  },
  {
    path: 'detalleBalanceFinanciero/:id',
    component: DetalleBalanceFinancieroRlcComponent
  },
  {
    path: 'detalleOrdengiro/:id',
    component: DetalleOgRlcComponent
  },
  {
    path: 'detalleTraslado/:id',
    component: DetalleTrasladoRlcComponent
  },
  {
    path: 'verDetalleEditarSolicitud/:id',
    component: GestionarSolicitudRlcComponent
  },
  {
    path: 'verDetalleSolicitud/:id',
    component: GestionarSolicitudRlcComponent
  },
  {
    path: 'gestionarSolicitud/:id/verDetalleBalance/:proyectoId',
    component: DetalleBalanceFinancieroRlcComponent
  },
  {
    path: 'verDetalleEditarSolicitud/:id/verDetalleBalance/:proyectoId',
    component: DetalleBalanceFinancieroRlcComponent
  },
  {
    path: 'verDetalleSolicitud/:id/verDetalleBalance/:proyectoId',
    component: DetalleBalanceFinancieroRlcComponent
  },
  {
    path: 'gestionarSolicitud/:id/verDetalleBalance/:proyectoId/recursosComprometidos',
    component: RecursosComproPagadosRlcComponent
  },
  {
    path: 'verDetalleEditarSolicitud/:id/verDetalleBalance/:proyectoId/recursosComprometidos',
    component: RecursosComproPagadosRlcComponent
  },
  {
    path: 'verDetalleSolicitud/:id/verDetalleBalance/:proyectoId/recursosComprometidos',
    component: RecursosComproPagadosRlcComponent
  },
  {
    path: 'gestionarSolicitud/:id/verDetalleBalance/:proyectoId/ejecucionFinanciera',
    component: EjecucionFinancieraRlcComponent
  },
  {
    path: 'verDetalleEditarSolicitud/:id/verDetalleBalance/:proyectoId/ejecucionFinanciera',
    component: EjecucionFinancieraRlcComponent
  },
  {
    path: 'verDetalleSolicitud/:id/verDetalleBalance/:proyectoId/ejecucionFinanciera',
    component: EjecucionFinancieraRlcComponent
  },
  {
    path: 'gestionarSolicitud/:id/verDetalleBalance/:proyectoId/trasladoRecursos',
    component: TrasladoRecursosRlcComponent
  },
  {
    path: 'verDetalleEditarSolicitud/:id/verDetalleBalance/:proyectoId/trasladoRecursos',
    component: TrasladoRecursosRlcComponent
  },
  {
    path: 'verDetalleSolicitud/:id/verDetalleBalance/:proyectoId/trasladoRecursos',
    component: TrasladoRecursosRlcComponent
  },
  {
    path: 'gestionarSolicitud/:id/verDetalleBalance/:proyectoId/liberacionSaldo',
    component: VerLiberacionSaldosComponent
  },
  {
    path: 'verDetalleEditarSolicitud/:id/verDetalleBalance/:proyectoId/liberacionSaldo',
    component: VerLiberacionSaldosComponent
  },
  {
    path: 'verDetalleSolicitud/:id/verDetalleBalance/:proyectoId/liberacionSaldo',
    component: VerLiberacionSaldosComponent
  },
  {
    path: 'gestionarSolicitud/:id/verDetalleInformeFinal/:proyectoId',
    component: DetalleInformeFinalRlcComponent
  },
  {
    path: 'verDetalleEditarSolicitud/:id/verDetalleInformeFinal/:proyectoId',
    component: DetalleInformeFinalRlcComponent
  },
  {
    path: 'verDetalleSolicitud/:id/verDetalleInformeFinal/:proyectoId',
    component: DetalleInformeFinalRlcComponent
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes),NgxCurrencyModule.forRoot(customCurrencyMaskConfig)],
  exports: [RouterModule]
})
export class RegistrarLiquidacionContratoRoutingModule { }
