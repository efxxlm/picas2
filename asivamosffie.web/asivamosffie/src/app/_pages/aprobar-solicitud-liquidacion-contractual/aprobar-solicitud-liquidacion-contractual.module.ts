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

@NgModule({
  declarations: [HomeComponent, ExpansionPanelComponent, TablaLiquidacionContratoObraComponent, AprobarRequisitosLiquidacionComponent, ActualizacionPolizaComponent, FormObservacionActualizacionPolizaComponent, TablaBalanceFinancieroComponent, AprobarBalanceComponent, FormObservacionBalanceComponent],
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
