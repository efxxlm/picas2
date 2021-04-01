import { TerceroCausacionComponent } from './components/tercero-causacion/tercero-causacion.component';
import { TablaDatosSolicitudComponent } from './components/tabla-datos-solicitud/tabla-datos-solicitud.component';
import { TablaAportantesComponent } from './components/tabla-aportantes/tabla-aportantes.component';
import { SoporteOrdenGiroComponent } from './components/soporte-orden-giro/soporte-orden-giro.component';
import { InformacionGeneralComponent } from './components/informacion-general/informacion-general.component';
import { DetalleGiroComponent } from './components/detalle-giro/detalle-giro.component';
import { DescuentosDireccionTecnicaComponent } from './components/descuentos-direccion-tecnica/descuentos-direccion-tecnica.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TramitarOrdenGiroRoutingModule } from './tramitar-orden-giro-routing.module';
import { TramitarOrdenGiroComponent } from './components/tramitar-orden-giro/tramitar-orden-giro.component';
import { TablaTramitarOrdenGiroComponent } from './components/tabla-tramitar-orden-giro/tabla-tramitar-orden-giro.component';
import { FormTramitarOrdenGiroComponent } from './components/form-tramitar-orden-giro/form-tramitar-orden-giro.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { DialogDescargarOrdenGiroComponent } from './components/dialog-descargar-orden-giro/dialog-descargar-orden-giro.component';
import { FormOrigenComponent } from './components/form-origen/form-origen.component';


@NgModule({
  declarations: [TerceroCausacionComponent, TablaDatosSolicitudComponent, TablaAportantesComponent, SoporteOrdenGiroComponent, InformacionGeneralComponent, DetalleGiroComponent, DescuentosDireccionTecnicaComponent, TramitarOrdenGiroComponent, TablaTramitarOrdenGiroComponent, FormTramitarOrdenGiroComponent, DialogDescargarOrdenGiroComponent, FormOrigenComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    QuillModule.forRoot(),
    TramitarOrdenGiroRoutingModule
  ]
})
export class TramitarOrdenGiroModule { }
