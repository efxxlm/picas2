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



@NgModule({
  declarations: [GenerarOrdenGiroComponent, FormGenerarOrdenGiroComponent, AccordionInfoGeneralGogComponent, AccordionDetalleGiroGogComponent, TablaDatosSolicitudGogComponent, TablaDatosDdpGogComponent, FormTerceroGiroGogComponent, TablaPorcntjParticGogComponent, TablaInfoFuenterecGogComponent, FormEstrategPagosGogComponent, DescDirTecnicaGogComponent],
  imports: [
    GenerarOrdenGiroRoutingModule,
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot(),
  ]
})
export class GenerarOrdenGiroModule { }
