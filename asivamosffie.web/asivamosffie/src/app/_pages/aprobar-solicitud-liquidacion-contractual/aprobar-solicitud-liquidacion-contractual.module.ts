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
    ObservacionesSupervisorComponent
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
