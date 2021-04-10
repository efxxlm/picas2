import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';

import { QuillModule } from 'ngx-quill';

import { GestionarProcesosDeSeleccionRoutingModule } from './gestionar-procesos-de-seleccion-routing.module';
import { BtnRegistrarComponent } from './components/btn-registrar/btn-registrar.component';
import { RegistrarNuevoComponent } from './components/registrar-nuevo/registrar-nuevo.component';
import { SeccionPrivadaComponent } from './components/seccion-privada/seccion-privada.component';
import { FormDescripcionDelProcesoDeSeleccionComponent } from './components/form-descripcion-del-proceso-de-seleccion/form-descripcion-del-proceso-de-seleccion.component';
import { FormEstudioDeMercadoComponent } from './components/form-estudio-de-mercado/form-estudio-de-mercado.component';
import { FormDatosProponentesSeleccionadosComponent } from './components/form-datos-proponentes-seleccionados/form-datos-proponentes-seleccionados.component';
import { TablaProcesosComponent } from './components/tabla-procesos/tabla-procesos.component';
import { InvitacionCerradaComponent } from './components/invitacion-cerrada/invitacion-cerrada.component';
import { InvitacionAbiertaComponent } from './components/invitacion-abierta/invitacion-abierta.component';
import { FormSeleccionProponenteAInvitarComponent } from './components/form-seleccion-proponente-a-invitar/form-seleccion-proponente-a-invitar.component';
import { FormEvaluacionComponent } from './components/form-evaluacion/form-evaluacion.component';
import { FormDatosProponentesSeleccionadosInvitacionCerradaComponent } from './components/proponentes-seleccionados-invitacion-cerrada/proponentes-seleccionados-invitacion-cerrada.component';
import { FormOrdenDeElegibilidadComponent } from './components/form-orden-de-elegibilidad/form-orden-de-elegibilidad.component';
import { CargarOrdenDeElegibilidadComponent } from './components/cargar-orden-de-elegibilidad/cargar-orden-de-elegibilidad.component';
import { TablaOrdenDeElegibilidadComponent } from './components/tabla-orden-de-elegibilidad/tabla-orden-de-elegibilidad.component';
import { VerDetalleTablaProcesosComponent } from './components/ver-detalle-tabla-procesos/ver-detalle-tabla-procesos.component';
import { MonitorearCronogramaComponent } from './components/monitorear-cronograma/monitorear-cronograma.component';
import { TablaCronogramaComponent } from './components/tabla-cronograma/tabla-cronograma.component';
import { TablaDetalleCronogramaComponent } from './components/tabla-detalle-cronograma/tabla-detalle-cronograma.component';
import { RegistrarSeguimientoCronogramaComponent } from './components/registrar-seguimiento-cronograma/registrar-seguimiento-cronograma.component';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatCardModule } from '@angular/material/card';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { FormDatosProponentesNuevoComponent } from './components/form-datos-proponentes-nuevo/form-datos-proponentes-nuevo.component';
import { VerObservacionesComponent } from './components/ver-observaciones/ver-observaciones.component';
import { DialogDevolucionComponent } from './components/dialog-devolucion/dialog-devolucion.component';

@NgModule({
  declarations: [
    BtnRegistrarComponent,
    RegistrarNuevoComponent,
    SeccionPrivadaComponent,
    FormDescripcionDelProcesoDeSeleccionComponent,
    FormEstudioDeMercadoComponent,
    FormDatosProponentesSeleccionadosComponent,
    TablaProcesosComponent,
    InvitacionCerradaComponent,
    InvitacionAbiertaComponent,
    FormSeleccionProponenteAInvitarComponent,
    FormEvaluacionComponent,
    FormDatosProponentesSeleccionadosInvitacionCerradaComponent,
    FormOrdenDeElegibilidadComponent,
    CargarOrdenDeElegibilidadComponent,
    TablaOrdenDeElegibilidadComponent,
    VerDetalleTablaProcesosComponent,
    VerObservacionesComponent,
    MonitorearCronogramaComponent,
    TablaCronogramaComponent,
    TablaDetalleCronogramaComponent,
    RegistrarSeguimientoCronogramaComponent,
    FormDatosProponentesNuevoComponent,
    DialogDevolucionComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    GestionarProcesosDeSeleccionRoutingModule,
    QuillModule.forRoot(),
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatRadioModule,
    MatCardModule,
    CurrencyMaskModule,
    MatAutocompleteModule
  ],
})
export class GestionarProcesosDeSeleccionModule { }
