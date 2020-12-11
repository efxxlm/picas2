import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrarActuacionControvContrcComponent } from './components/registrar-actuacion-controv-contrc/registrar-actuacion-controv-contrc.component';
import { MaterialModule } from 'src/app/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { RegistrarActuacionControvContrcRoutingModule } from './registrar-actuacion-controv-contrc-routing.module';
import { TablaControversiasRaccComponent } from './components/tabla-controversias-racc/tabla-controversias-racc.component';
import { ActualizarTramiteRaccComponent } from './components/actualizar-tramite-racc/actualizar-tramite-racc.component';
import { RegistrarAvanceActuaDerivadasComponent } from './components/registrar-avance-actua-derivadas/registrar-avance-actua-derivadas.component';
import { VerdetalleeditAvanceActuaDerivadasComponent } from './components/verdetalleedit-avance-actua-derivadas/verdetalleedit-avance-actua-derivadas.component';
import { DetalleAvanceActuaDerivadasComponent } from './components/detalle-avance-actua-derivadas/detalle-avance-actua-derivadas.component';



@NgModule({
  declarations: [RegistrarActuacionControvContrcComponent, TablaControversiasRaccComponent, ActualizarTramiteRaccComponent, RegistrarAvanceActuaDerivadasComponent, VerdetalleeditAvanceActuaDerivadasComponent, DetalleAvanceActuaDerivadasComponent],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot(),
    RegistrarActuacionControvContrcRoutingModule
  ]
})
export class RegistrarActuacionControvContrcModule { }
