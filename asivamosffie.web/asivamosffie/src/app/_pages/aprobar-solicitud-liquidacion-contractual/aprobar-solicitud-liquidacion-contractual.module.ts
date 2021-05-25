import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { AprobarSolicitudLiquidacionContractualRoutingModule } from './aprobar-solicitud-liquidacion-contractual-routing.module';
import { HomeComponent } from './components/home/home.component';
import { ExpansionPanelComponent } from './components/expansion-panel/expansion-panel.component';
import { TablaLiquidacionContratoObraComponent } from './components/tabla-liquidacion-contrato-obra/tabla-liquidacion-contrato-obra.component';
import { AprobarRequisitosLiquidacionComponent } from './components/aprobar-requisitos-liquidacion/aprobar-requisitos-liquidacion.component';
import { ActualizacionPolizaComponent } from './components/actualizacion-poliza/actualizacion-poliza.component';
import { FormObservacionActualizacionPolizaComponent } from './components/form-observacion-actualizacion-poliza/form-observacion-actualizacion-poliza.component';
import { TablaBalanceFinancieroComponent } from './components/tabla-balance-financiero/tabla-balance-financiero.component';
import { AprobarBalanceComponent } from './components/aprobar-balance/aprobar-balance.component';
import { FormObservacionBalanceComponent } from './components/form-observacion-balance/form-observacion-balance.component';
import { RecursosComprometidosPagadosComponent } from './components/recursos-comprometidos-pagados/recursos-comprometidos-pagados.component';
import { DetalleRecursosComprometidosComponent } from './components/detalle-recursos-comprometidos/detalle-recursos-comprometidos.component';
import { DetalleContratistasComponent } from './components/detalle-contratistas/detalle-contratistas.component';
import { TablaRecursosComprometidosComponent } from './components/tabla-recursos-comprometidos/tabla-recursos-comprometidos.component';
import { TablaFuentesYRecursosComponent } from './components/tabla-fuentes-y-recursos/tabla-fuentes-y-recursos.component';
import { VerDetalleComponent } from './components/ver-detalle/ver-detalle.component';
import { VerEjecucionFinancieraComponent } from './components/ver-ejecucion-financiera/ver-ejecucion-financiera.component';
import { VerTrasladosRecursosComponent } from './components/ver-traslados-recursos/ver-traslados-recursos.component';
import { DetalleTrasladosComponent } from './components/detalle-traslados/detalle-traslados.component';
import { TablaInformeFinalComponent } from './components/tabla-informe-final/tabla-informe-final.component';
import { AprobarInformeFinalComponent } from './components/aprobar-informe-final/aprobar-informe-final.component';
import { TablaInformeAnexosComponent } from './components/tabla-informe-anexos/tabla-informe-anexos.component';
import { ObservacionesInformeFinalComponent } from './components/observaciones-informe-final/observaciones-informe-final.component';
import { TablaLiquidacionContratoInterventoriaComponent } from './components/tabla-liquidacion-contrato-interventoria/tabla-liquidacion-contrato-interventoria.component';
import { ObservacionesSupervisorComponent } from './components/observaciones-supervisor/observaciones-supervisorcomponent';
import { TablaTotalOrdenesDeGiroComponent } from './components/tabla-total-ordenes-de-giro/tabla-total-ordenes-de-giro.component';
import { RegistrarTrasladoGbftrecComponent } from './components/registrar-traslado-gbftrec/registrar-traslado-gbftrec.component';
import { TablaPorcParticipacionGbftrecComponent } from './components/tabla-porc-participacion-gbftrec/tabla-porc-participacion-gbftrec.component';
import { ControlSolicitudesTrasladoGbftrecComponent } from './components/control-solicitudes-traslado-gbftrec/control-solicitudes-traslado-gbftrec.component';
import { FormOrdenGiroComponent } from './components/form-orden-giro/form-orden-giro.component';
import { DatosDdpDrpGbftrecComponent } from './components/datos-ddp-drp-gbftrec/datos-ddp-drp-gbftrec.component';
import { DatosSolicitudGbftrecComponent } from './components/datos-solicitud-gbftrec/datos-solicitud-gbftrec.component';
import { TablaInfofrecursosGbftrecComponent } from './components/tabla-infofrecursos-gbftrec/tabla-infofrecursos-gbftrec.component';
import { FormTerceroCausacionComponent } from './components/form-tercero-causacion/form-tercero-causacion.component';
import { FormDescuentosDireccionTecnicaComponent } from './components/form-descuentos-direccion-tecnica/form-descuentos-direccion-tecnica.component';
import { FormOrdenGiroSeleccionadaComponent } from './components/form-orden-giro-seleccionada/form-orden-giro-seleccionada.component';

@NgModule({
  declarations: [
    HomeComponent,
    ExpansionPanelComponent,
    TablaLiquidacionContratoObraComponent,
    AprobarRequisitosLiquidacionComponent,
    ActualizacionPolizaComponent,
    FormObservacionActualizacionPolizaComponent,
    TablaBalanceFinancieroComponent,
    AprobarBalanceComponent,
    FormObservacionBalanceComponent,
    RecursosComprometidosPagadosComponent,
    DetalleRecursosComprometidosComponent,
    DetalleContratistasComponent,
    TablaRecursosComprometidosComponent,
    TablaFuentesYRecursosComponent,
    VerDetalleComponent,
    VerEjecucionFinancieraComponent,
    VerTrasladosRecursosComponent,
    DetalleTrasladosComponent,
    TablaInformeFinalComponent,
    AprobarInformeFinalComponent,
    TablaInformeAnexosComponent,
    ObservacionesInformeFinalComponent,
    TablaLiquidacionContratoInterventoriaComponent,
    ObservacionesSupervisorComponent,
    TablaTotalOrdenesDeGiroComponent,
    RegistrarTrasladoGbftrecComponent,
    ControlSolicitudesTrasladoGbftrecComponent,
    FormOrdenGiroComponent,
    DatosDdpDrpGbftrecComponent,
    DatosSolicitudGbftrecComponent,
    TablaInfofrecursosGbftrecComponent,
    TablaPorcParticipacionGbftrecComponent,
    FormTerceroCausacionComponent,
    FormDescuentosDireccionTecnicaComponent,
    FormOrdenGiroSeleccionadaComponent
  ],
  imports: [
    CommonModule,
    AprobarSolicitudLiquidacionContractualRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class AprobarSolicitudLiquidacionContractualModule {}
