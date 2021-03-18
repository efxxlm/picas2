import { TerceroCausacionComponent } from './components/tercero-causacion/tercero-causacion.component';
import { TablaDatosSolicitudComponent } from './components/tabla-datos-solicitud/tabla-datos-solicitud.component';
import { TablaAportantesComponent } from './components/tabla-aportantes/tabla-aportantes.component';
import { SoporteOrdenGiroComponent } from './components/soporte-orden-giro/soporte-orden-giro.component';
import { ObservacionesComponent } from './components/observaciones/observaciones.component';
import { InformacionGeneralComponent } from './components/informacion-general/informacion-general.component';
import { DialogEnviarAprobacionComponent } from './components/dialog-enviar-aprobacion/dialog-enviar-aprobacion.component';
import { DetalleGiroComponent } from './components/detalle-giro/detalle-giro.component';
import { DescuentosDireccionTecnicaComponent } from './components/descuentos-direccion-tecnica/descuentos-direccion-tecnica.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AprobarOrdenGiroRoutingModule } from './aprobar-orden-giro-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { AprobarOrdenGiroComponent } from './components/aprobar-orden-giro/aprobar-orden-giro.component';
import { TablaAprobarOrdenGiroComponent } from './components/tabla-aprobar-orden-giro/tabla-aprobar-orden-giro.component';
import { FormAprobarOrdenGiroComponent } from './components/form-aprobar-orden-giro/form-aprobar-orden-giro.component';


@NgModule({
  declarations: [TerceroCausacionComponent, TablaDatosSolicitudComponent, TablaAportantesComponent, SoporteOrdenGiroComponent, ObservacionesComponent, InformacionGeneralComponent, DialogEnviarAprobacionComponent, DetalleGiroComponent, DescuentosDireccionTecnicaComponent, AprobarOrdenGiroComponent, TablaAprobarOrdenGiroComponent, FormAprobarOrdenGiroComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    QuillModule.forRoot(),
    AprobarOrdenGiroRoutingModule
  ]
})
export class AprobarOrdenGiroModule { }
