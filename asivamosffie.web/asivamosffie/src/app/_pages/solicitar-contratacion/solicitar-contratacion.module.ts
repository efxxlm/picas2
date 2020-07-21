import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SolicitarContratacionRoutingModule } from './solicitar-contratacion-routing.module';
import { SolicitarContratacionComponent } from './components/solicitar-contratacion/solicitar-contratacion.component';

import { MaterialModule } from './../../material/material.module';
import { FormSolicitarContratacionComponent } from './components/form-solicitar-contratacion/form-solicitar-contratacion.component';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatCardModule } from '@angular/material/card';
import { ReactiveFormsModule } from '@angular/forms';
import { TablaResultadosComponent } from './components/tabla-resultados/tabla-resultados.component';


@NgModule({
  declarations: [SolicitarContratacionComponent, FormSolicitarContratacionComponent, TablaResultadosComponent],
  imports: [
    CommonModule,
    SolicitarContratacionRoutingModule,
    MaterialModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatRadioModule,
    MatCardModule,
    ReactiveFormsModule,
  ]
})
export class SolicitarContratacionModule { }
