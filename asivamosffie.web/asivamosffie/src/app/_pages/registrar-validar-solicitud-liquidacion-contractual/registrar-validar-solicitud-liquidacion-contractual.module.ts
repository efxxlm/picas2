import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { RegistrarValidarSolicitudLiquidacionContractualRoutingModule } from './registrar-validar-solicitud-liquidacion-contractual-routing.module';
import { HomeComponent } from './components/home/home.component';
import { TablaLiquidacionObraComponent } from './components/tabla-liquidacion-obra/tabla-liquidacion-obra.component';
import { TablaLiquidacionInterventoriaComponent } from './components/tabla-liquidacion-interventoria/tabla-liquidacion-interventoria.component';
import { ValidarRequisitosLiquidacionComponent } from './components/validar-requisitos-liquidacion/validar-requisitos-liquidacion.component';
import { ActualizacionPolizaComponent } from './components/actualizacion-poliza/actualizacion-poliza.component';
import { ObservacionesActualizacionPolizaComponent } from './components/observaciones-actualizacion-poliza/observaciones-actualizacion-poliza.component';
import { TablaBalanceFinancieroComponent } from './components/tabla-balance-financiero/tabla-balance-financiero.component';
import { ValidarBalanceComponent } from './components/validar-balance/validar-balance.component';
import { ObservacionesBalanceComponent } from './components/observaciones-balance/observaciones-balance.component';
import { RecursosComprometidosPagadosComponent } from './components/recursos-comprometidos-pagados/recursos-comprometidos-pagados.component';
import { VerDetalleComponent } from './components/ver-detalle/ver-detalle.component';
import { VerEjecucionFinancieraComponent } from './components/ver-ejecucion-financiera/ver-ejecucion-financiera.component';
import { VerTrasladosRecursosComponent } from './components/ver-traslados-recursos/ver-traslados-recursos.component';
import { DetalleTrasladosComponent } from './components/detalle-traslados/detalle-traslados.component';
import { TablaInformeFinalComponent } from './components/tabla-informe-final/tabla-informe-final.component';
import { ValidarInformeFinalComponent } from './components/validar-informe-final/validar-informe-final.component';
import { ObservacionesInformeFinalComponent } from './components/observaciones-informe-final/observaciones-informe-final.component';
import { TablaInformeAnexosComponent } from './components/tabla-informe-anexos/tabla-informe-anexos.component';

@NgModule({
  declarations: [
    HomeComponent,
    TablaLiquidacionObraComponent,
    TablaLiquidacionInterventoriaComponent,
    ValidarRequisitosLiquidacionComponent,
    ActualizacionPolizaComponent,
    ObservacionesActualizacionPolizaComponent,
    TablaBalanceFinancieroComponent,
    ValidarBalanceComponent,
    ObservacionesBalanceComponent,
    RecursosComprometidosPagadosComponent,
    VerDetalleComponent,
    VerEjecucionFinancieraComponent,
    VerTrasladosRecursosComponent,
    DetalleTrasladosComponent,
    TablaInformeFinalComponent,
    ValidarInformeFinalComponent,
    ObservacionesInformeFinalComponent,
    TablaInformeAnexosComponent
  ],
  imports: [
    CommonModule,
    RegistrarValidarSolicitudLiquidacionContractualRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class RegistrarValidarSolicitudLiquidacionContractualModule { }
