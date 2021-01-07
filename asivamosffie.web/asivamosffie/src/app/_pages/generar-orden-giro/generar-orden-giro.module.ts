import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GenerarOrdenGiroRoutingModule } from './generar-orden-giro-routing.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { GenerarOrdenGiroComponent } from './components/generar-orden-giro/generar-orden-giro.component';
import { FormGenerarOrdenGiroComponent } from './components/form-generar-orden-giro/form-generar-orden-giro.component';
import { AccordionInfoGeneralGogComponent } from './components/accordion-info-general-gog/accordion-info-general-gog.component';
import { AccordionDetalleGiroGogComponent } from './components/accordion-detalle-giro-gog/accordion-detalle-giro-gog.component';
import { TablaDatosSolicitudGogComponent } from './components/tabla-datos-solicitud-gog/tabla-datos-solicitud-gog.component';
import { TablaDatosDdpGogComponent } from './components/tabla-datos-ddp-gog/tabla-datos-ddp-gog.component';
import { FormTerceroGiroGogComponent } from './components/form-tercero-giro-gog/form-tercero-giro-gog.component';
import { TablaPorcntjParticGogComponent } from './components/tabla-porcntj-partic-gog/tabla-porcntj-partic-gog.component';
import { TablaInfoFuenterecGogComponent } from './components/tabla-info-fuenterec-gog/tabla-info-fuenterec-gog.component';
import { FormEstrategPagosGogComponent } from './components/form-estrateg-pagos-gog/form-estrateg-pagos-gog.component';
import { DescDirTecnicaGogComponent } from './components/desc-dir-tecnica-gog/desc-dir-tecnica-gog.component';
import { FormCostoVariableGogComponent } from './components/form-costo-variable-gog/form-costo-variable-gog.component';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { FormTipopago3GogComponent } from './components/form-tipopago3-gog/form-tipopago3-gog.component';
import { TerceroCausacionGogComponent } from './components/tercero-causacion-gog/tercero-causacion-gog.component';
import { FormTipopago3TercGogComponent } from './components/form-tipopago3-terc-gog/form-tipopago3-terc-gog.component';
import { FormCostoVariableTercGogComponent } from './components/form-costo-variable-terc-gog/form-costo-variable-terc-gog.component';
import { ObservacionesOrdenGiroGogComponent } from './components/observaciones-orden-giro-gog/observaciones-orden-giro-gog.component';
import { SoporteOrdenGiroGogComponent } from './components/soporte-orden-giro-gog/soporte-orden-giro-gog.component';
import { VerDetalleEditOrdenGiroGogComponent } from './components/ver-detalle-edit-orden-giro-gog/ver-detalle-edit-orden-giro-gog.component';
import { DialogDevolverSolPagoGogComponent } from './components/dialog-devolver-sol-pago-gog/dialog-devolver-sol-pago-gog.component';
import { DetalleOrdenGiroGogComponent } from './components/detalle-orden-giro-gog/detalle-orden-giro-gog.component';



@NgModule({
  declarations: [GenerarOrdenGiroComponent, FormGenerarOrdenGiroComponent, AccordionInfoGeneralGogComponent, AccordionDetalleGiroGogComponent, TablaDatosSolicitudGogComponent, TablaDatosDdpGogComponent, FormTerceroGiroGogComponent, TablaPorcntjParticGogComponent, TablaInfoFuenterecGogComponent, FormEstrategPagosGogComponent, DescDirTecnicaGogComponent, FormCostoVariableGogComponent, FormTipopago3GogComponent, TerceroCausacionGogComponent, FormTipopago3TercGogComponent, FormCostoVariableTercGogComponent, ObservacionesOrdenGiroGogComponent, SoporteOrdenGiroGogComponent, VerDetalleEditOrdenGiroGogComponent, DialogDevolverSolPagoGogComponent, DetalleOrdenGiroGogComponent],
  imports: [
    GenerarOrdenGiroRoutingModule,
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot(),
    CurrencyMaskModule
  ]
})
export class GenerarOrdenGiroModule { }
