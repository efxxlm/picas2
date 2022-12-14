import { FormEstrategPagosGogComponent } from './components/form-estrateg-pagos-gog/form-estrateg-pagos-gog.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VerificarOrdenGiroRoutingModule } from './verificar-orden-giro-routing.module';
import { VerificarOrdenGiroComponent } from './components/verificar-orden-giro/verificar-orden-giro.component';
import { TablaVerificarOrdenGiroComponent } from './components/tabla-verificar-orden-giro/tabla-verificar-orden-giro.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { FormVerificarOrdenGiroComponent } from './components/form-verificar-orden-giro/form-verificar-orden-giro.component';
import { InformacionGeneralComponent } from './components/informacion-general/informacion-general.component';
import { TablaDatosSolicitudComponent } from './components/tabla-datos-solicitud/tabla-datos-solicitud.component';
import { TablaAportantesComponent } from './components/tabla-aportantes/tabla-aportantes.component';
import { DetalleGiroComponent } from './components/detalle-giro/detalle-giro.component';
import { DescuentosDireccionTecnicaComponent } from './components/descuentos-direccion-tecnica/descuentos-direccion-tecnica.component';
import { TerceroCausacionComponent } from './components/tercero-causacion/tercero-causacion.component';
import { ObservacionesComponent } from './components/observaciones/observaciones.component';
import { SoporteOrdenGiroComponent } from './components/soporte-orden-giro/soporte-orden-giro.component';
import { DialogEnviarAprobacionComponent } from './components/dialog-enviar-aprobacion/dialog-enviar-aprobacion.component';
import { FormOrigenComponent } from './components/form-origen/form-origen.component';
import { TablaPorcntjParticGogComponent } from './components/tabla-porcntj-partic-gog/tabla-porcntj-partic-gog.component';
import { TablaInfoFuenterecGogComponent } from './components/tabla-info-fuenterec-gog/tabla-info-fuenterec-gog.component';
import { AmortizacionPagoComponent } from './components/amortizacion-pago/amortizacion-pago.component';


@NgModule({
  declarations: [ FormEstrategPagosGogComponent, VerificarOrdenGiroComponent, TablaVerificarOrdenGiroComponent, FormVerificarOrdenGiroComponent, InformacionGeneralComponent, TablaDatosSolicitudComponent, TablaAportantesComponent, DetalleGiroComponent, DescuentosDireccionTecnicaComponent, TerceroCausacionComponent, ObservacionesComponent, SoporteOrdenGiroComponent, DialogEnviarAprobacionComponent, FormOrigenComponent, TablaPorcntjParticGogComponent, TablaInfoFuenterecGogComponent, AmortizacionPagoComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    QuillModule.forRoot(),
    VerificarOrdenGiroRoutingModule
  ]
})
export class VerificarOrdenGiroModule { }
