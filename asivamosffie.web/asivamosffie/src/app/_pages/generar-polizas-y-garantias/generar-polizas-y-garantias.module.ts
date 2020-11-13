import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { QuillModule } from 'ngx-quill';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { ReactiveFormsModule } from '@angular/forms';

import { GenerarPolizasYGarantiasRoutingModule } from './generar-polizas-y-garantias-routing.module';
import { MenuComponent } from './components/menu/menu.component';
import { TablaSinRadicacionDePolizasComponent } from './components/tabla-sin-radicacion-de-polizas/tabla-sin-radicacion-de-polizas.component';
import { TablaEnRevisionDePolizasComponent } from './components/tabla-en-revision-de-polizas/tabla-en-revision-de-polizas.component';
import { TablaConPolizaObservadaYDevueltaComponent } from './components/tabla-con-poliza-observada-y-devuelta/tabla-con-poliza-observada-y-devuelta.component';
import { TablaConAprobacionDePolizasComponent } from './components/tabla-con-aprobacion-de-polizas/tabla-con-aprobacion-de-polizas.component';
import { GestionarPolizasComponent } from './components/gestionar-polizas/gestionar-polizas.component';
import { EditarEnRevisionComponent } from './components/editar-en-revision/editar-en-revision.component';
import { EditarObservadaODevueltaComponent } from './components/editar-observada-o-devuelta/editar-observada-o-devuelta.component';
import { VerDetallePolizaComponent } from './components/ver-detalle-poliza/ver-detalle-poliza.component';
import { TablaHistorialObservacionesPolizaComponent } from './components/tabla-historial-observaciones-poliza/tabla-historial-observaciones-poliza.component';


@NgModule({
  declarations: [
    MenuComponent,
    TablaSinRadicacionDePolizasComponent,
    TablaEnRevisionDePolizasComponent,
    TablaConPolizaObservadaYDevueltaComponent,
    TablaConAprobacionDePolizasComponent,
    GestionarPolizasComponent,
    EditarEnRevisionComponent,
    EditarObservadaODevueltaComponent,
    VerDetallePolizaComponent,
    TablaHistorialObservacionesPolizaComponent
  ],
  imports: [
    CommonModule,
    GenerarPolizasYGarantiasRoutingModule,
    MaterialModule,
    QuillModule.forRoot(),
    ReactiveFormsModule,
    CurrencyMaskModule
  ]
})
export class GenerarPolizasYGarantiasModule { }
