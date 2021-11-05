import { NgModule } from '@angular/core';
import { QuillModule } from 'ngx-quill';
import { CommonModule } from '@angular/common';

import { GestionarProcesosContractualesRoutingModule } from './gestionar-procesos-contractuales-routing.module';
import { ProcesosContractualesComponent } from './components/procesos-contractuales/procesos-contractuales.component';
import { MaterialModule } from '../../material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TablaSolicitudesSinTramitarComponent } from './components/tabla-solicitudes-sin-tramitar/tabla-solicitudes-sin-tramitar.component';
import { FormContratacionComponent } from './components/form-contratacion/form-contratacion.component';
import { FormAportantesComponent } from './components/form-aportantes/form-aportantes.component';
import { FormModificacionContractualComponent } from './components/form-modificacion-contractual/form-modificacion-contractual.component';
import { TablaProyectosAsociadosComponent } from './components/tabla-proyectos-asociados/tabla-proyectos-asociados.component';
import { FormLiquidacionComponent } from './components/form-liquidacion/form-liquidacion.component';
import { TablaRecursosCompartidosComponent } from './components/tabla-recursos-compartidos/tabla-recursos-compartidos.component';
import { FormRecursosCompartidosComponent } from './components/form-recursos-compartidos/form-recursos-compartidos.component';
import { TablaRecursosAportantesComponent } from './components/tabla-recursos-aportantes/tabla-recursos-aportantes.component';
import { FormRecursosPagadosComponent } from './components/form-recursos-pagados/form-recursos-pagados.component';
import { TablaRecursosPagadosComponent } from './components/tabla-recursos-pagados/tabla-recursos-pagados.component';
import { TablaOtrosDescuentosComponent } from './components/tabla-otros-descuentos/tabla-otros-descuentos.component';
import { FormRegistroTramiteComponent } from './components/form-registro-tramite/form-registro-tramite.component';
import { TablaSolicitudesEnviadasComponent } from './components/tabla-solicitudes-enviadas/tabla-solicitudes-enviadas.component';
import { DomSafePipe } from '../../_pipes/dom-safe.pipe';
import { TablaRegistradosComponent } from './components/tabla-registrados/tabla-registrados.component';
import { FormContratacionRegistradosComponent } from './components/form-contratacion-registrados/form-contratacion-registrados.component';
import { TablaInformeAnexosComponent } from './components/tabla-informe-anexos/tabla-informe-anexos.component';
import { TablaSolicitudesDevueltasComponent } from './components/tabla-solicitudes-devueltas/tabla-solicitudes-devueltas.component';
import { DialogObservacionesComponent } from './components/dialog-observaciones/dialog-observaciones.component';
import { TablaFuentesUsosComponent } from './components/tabla-fuentes-usos/tabla-fuentes-usos.component';
import { TablaValtotalOgComponent } from './components/tabla-valtotal-og/tabla-valtotal-og.component';
import { ListaContratistasComponent } from './components/lista-contratistas/lista-contratistas.component';
import { AcordionRecursosComproPagadosComponent } from './components/acordion-recursos-compro-pagados/acordion-recursos-compro-pagados.component';
import { RecursosComproPagadosComponent } from './components/recursos-compro-pagados/recursos-compro-pagados.component';
import { TablaEjpresupuestalComponent } from './components/tabla-ejpresupuestal/tabla-ejpresupuestal.component';
import { TablaEjfinancieraComponent } from './components/tabla-ejfinanciera/tabla-ejfinanciera.component';
import { TablaLiberacionSaldoComponent } from './components/tabla-liberacion-saldo/tabla-liberacion-saldo.component';
import { VerLiberacionSaldosComponent } from './components/ver-liberacion-saldos/ver-liberacion-saldos.component';

@NgModule({
  declarations: [
    ProcesosContractualesComponent,
    TablaSolicitudesSinTramitarComponent,
    FormContratacionComponent,
    FormAportantesComponent,
    FormModificacionContractualComponent,
    TablaProyectosAsociadosComponent,
    FormLiquidacionComponent,
    TablaRecursosCompartidosComponent,
    FormRecursosCompartidosComponent,
    TablaRecursosAportantesComponent,
    FormRecursosPagadosComponent,
    TablaRecursosPagadosComponent,
    TablaOtrosDescuentosComponent,
    FormRegistroTramiteComponent,
    TablaRegistradosComponent,
    FormContratacionRegistradosComponent,
    TablaSolicitudesEnviadasComponent,
    DomSafePipe,
    TablaInformeAnexosComponent,
    TablaSolicitudesDevueltasComponent,
    DialogObservacionesComponent,
    TablaEjfinancieraComponent,
    TablaEjpresupuestalComponent,
    RecursosComproPagadosComponent,
    AcordionRecursosComproPagadosComponent,
    ListaContratistasComponent,
    TablaValtotalOgComponent,
    TablaFuentesUsosComponent,
    TablaLiberacionSaldoComponent,
    VerLiberacionSaldosComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot(),
    GestionarProcesosContractualesRoutingModule
  ],
  exports: [
    TablaOtrosDescuentosComponent
  ]
})
export class GestionarProcesosContractualesModule { }
