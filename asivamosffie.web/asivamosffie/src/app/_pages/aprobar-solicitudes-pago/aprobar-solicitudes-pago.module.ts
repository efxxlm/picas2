import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AprobarSolicitudesPagoRoutingModule } from './aprobar-solicitudes-pago-routing.module';
import { AprobarSolicitudesPagoComponent } from './components/aprobar-solicitudes-pago/aprobar-solicitudes-pago.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { FormAprobarSolicitudComponent } from './components/form-aprobar-solicitud/form-aprobar-solicitud.component';
import { DialogProyectosAsociadosAprobComponent } from './components/dialog-proyectos-asociados-aprob/dialog-proyectos-asociados-aprob.component';



@NgModule({
  declarations: [AprobarSolicitudesPagoComponent, FormAprobarSolicitudComponent, DialogProyectosAsociadosAprobComponent],
  imports: [
    CommonModule,
    AprobarSolicitudesPagoRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class AprobarSolicitudesPagoModule { }
