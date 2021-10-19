import { FormDescuentosDireccionTecnicaComponent } from './components/form-descuentos-direccion-tecnica/form-descuentos-direccion-tecnica.component';
import { FormTerceroCausacionComponent } from './components/form-tercero-causacion/form-tercero-causacion.component';
import { TablaPorcParticipacionGbftrecComponent } from './components/tabla-porc-participacion-gbftrec/tabla-porc-participacion-gbftrec.component';
import { TablaInfofrecursosGbftrecComponent } from './components/tabla-infofrecursos-gbftrec/tabla-infofrecursos-gbftrec.component';
import { DatosSolicitudGbftrecComponent } from './components/datos-solicitud-gbftrec/datos-solicitud-gbftrec.component';
import { DatosDdpDrpGbftrecComponent } from './components/datos-ddp-drp-gbftrec/datos-ddp-drp-gbftrec.component';
import { FormOrdenGiroSeleccionadaComponent } from './components/form-orden-giro-seleccionada/form-orden-giro-seleccionada.component';
import { FormOrdenGiroComponent } from './components/form-orden-giro/form-orden-giro.component';
import { ControlSolicitudesTrasladoGbftrecComponent } from './components/control-solicitudes-traslado-gbftrec/control-solicitudes-traslado-gbftrec.component';
import { RegistrarTrasladoGbftrecComponent } from './components/registrar-traslado-gbftrec/registrar-traslado-gbftrec.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { RegistrarSolicitudLiquidacionContractualRoutingModule } from './registrar-solicitud-liquidacion-contractual-routing.module';
import { HomeComponent } from './components/home/home.component';
import { ExpansionPanelComponent } from './components/expansion-panel/expansion-panel.component';
import { TablaLiquidacionContratoObraComponent } from './components/tabla-liquidacion-contrato-obra/tabla-liquidacion-contrato-obra.component';
import { ValidarRequerimientosLiquidacionesComponent } from './components/validar-requerimientos-liquidaciones/validar-requerimientos-liquidaciones.component';
import { ActualizacionPolizaComponent } from './components/actualizacion-poliza/actualizacion-poliza.component';
import { FormObservacionActualizacionPolizaComponent } from './components/form-observacion-actualizacion-poliza/form-observacion-actualizacion-poliza.component';
import { TablaBalanceFinancieroComponent } from './components/tabla-balance-financiero/tabla-balance-financiero.component';
import { ValidarBalanceComponent } from './components/validar-balance/validar-balance.component';
import { VerRecursosComponent } from './components/ver-recursos/ver-recursos.component';
import { FormObservacionBalanceComponent } from './components/form-observacion-balance/form-observacion-balance.component';
import { RecursosComprometidosPagadosComponent } from './components/recursos-comprometidos-pagados/recursos-comprometidos-pagados.component';
import { DetalleContratistasComponent } from './components/detalle-contratistas/detalle-contratistas.component';
import { DetalleRecursosComprometidosComponent } from './components/detalle-recursos-comprometidos/detalle-recursos-comprometidos.component';
import { TablaRecursosComprometidosComponent } from './components/tabla-recursos-comprometidos/tabla-recursos-comprometidos.component';
import { TablaFuentesYRecursosComponent } from './components/tabla-fuentes-y-recursos/tabla-fuentes-y-recursos.component';
import { TablaTotalOrdenesDeGiroComponent } from './components/tabla-total-ordenes-de-giro/tabla-total-ordenes-de-giro.component';
import { TablaLiquidacionContratoInterventoriaComponent } from './components/tabla-liquidacion-contrato-interventoria/tabla-liquidacion-contrato-interventoria.component';
import { TablaInformeFinalComponent } from './components/tabla-informe-final/tabla-informe-final.component';
import { ValidarInformeFinalComponent } from './components/validar-informe-final/validar-informe-final.component';
import { TablaInformeAnexosComponent } from './components/tabla-informe-anexos/tabla-informe-anexos.component';
import { FormObservacionInformeFinalComponent } from './components/form-observacion-informe-final/form-observacion-informe-final.component';
import { VerEjecucionFinancieraComponent } from './components/ver-ejecucion-financiera/ver-ejecucion-financiera.component';
import { VerTrasladosRecursosComponent } from './components/ver-traslados-recursos/ver-traslados-recursos.component';
import { DetalleTrasladosComponent } from './components/detalle-traslados/detalle-traslados.component';
import { TablaDatosSolicitudComponent } from './components/tabla-datos-solicitud/tabla-datos-solicitud.component';
import { TablaDatosDdpGogComponent } from './components/tabla-datos-ddp-gog/tabla-datos-ddp-gog.component';
import { TablaDatosDdpComponent } from './components/tabla-datos-ddp/tabla-datos-ddp.component';
import { FormTerceroGiroGogComponent } from './components/form-tercero-giro-gog/form-tercero-giro-gog.component';
import { TablaPorcntjParticGogComponent } from './components/tabla-porcntj-partic-gog/tabla-porcntj-partic-gog.component';
import { TablaInfoFuenterecGogComponent } from './components/tabla-info-fuenterec-gog/tabla-info-fuenterec-gog.component';
import { TablaEjfinancieraGbftrecComponent } from './components/tabla-ejfinanciera-gbftrec/tabla-ejfinanciera-gbftrec.component';
import { TablaEjpresupuestalGbftrecComponent } from './components/tabla-ejpresupuestal-gbftrec/tabla-ejpresupuestal-gbftrec.component';
import { VerLiberacionSaldosComponent } from './components/ver-liberacion-saldos/ver-liberacion-saldos.component';
import { TablaLiberacionSaldoComponent } from './components/tabla-liberacion-saldo/tabla-liberacion-saldo.component';

@NgModule({
  declarations: [
    RegistrarTrasladoGbftrecComponent,
    ControlSolicitudesTrasladoGbftrecComponent,
    FormOrdenGiroComponent,
    DatosDdpDrpGbftrecComponent,
    DatosSolicitudGbftrecComponent,
    TablaInfofrecursosGbftrecComponent,
    TablaPorcParticipacionGbftrecComponent,
    FormTerceroCausacionComponent,
    FormDescuentosDireccionTecnicaComponent,
    FormOrdenGiroSeleccionadaComponent,
    HomeComponent,
    ExpansionPanelComponent,
    TablaLiquidacionContratoObraComponent,
    ValidarRequerimientosLiquidacionesComponent,
    ActualizacionPolizaComponent,
    FormObservacionActualizacionPolizaComponent,
    TablaBalanceFinancieroComponent,
    ValidarBalanceComponent,
    VerRecursosComponent,
    FormObservacionBalanceComponent,
    RecursosComprometidosPagadosComponent,
    DetalleContratistasComponent,
    DetalleRecursosComprometidosComponent,
    TablaRecursosComprometidosComponent,
    TablaFuentesYRecursosComponent,
    TablaTotalOrdenesDeGiroComponent,
    TablaLiquidacionContratoInterventoriaComponent,
    TablaInformeFinalComponent,
    ValidarInformeFinalComponent,
    TablaInformeAnexosComponent,
    FormObservacionInformeFinalComponent,
    VerEjecucionFinancieraComponent,
    VerTrasladosRecursosComponent,
    DetalleTrasladosComponent,
    TablaDatosSolicitudComponent,
    TablaDatosDdpGogComponent,
    TablaDatosDdpComponent,
    FormTerceroGiroGogComponent,
    TablaPorcntjParticGogComponent,
    TablaInfoFuenterecGogComponent,
    TablaEjfinancieraGbftrecComponent,
    TablaEjpresupuestalGbftrecComponent,
    VerLiberacionSaldosComponent,
    TablaLiberacionSaldoComponent

  ],
  imports: [
    CommonModule,
    RegistrarSolicitudLiquidacionContractualRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class RegistrarSolicitudLiquidacionContractualModule {}
