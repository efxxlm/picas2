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

@NgModule({
  declarations: [
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
