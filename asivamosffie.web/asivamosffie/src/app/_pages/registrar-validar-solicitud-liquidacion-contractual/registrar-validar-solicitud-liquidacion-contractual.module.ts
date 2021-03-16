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
    VerDetalleComponent
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
