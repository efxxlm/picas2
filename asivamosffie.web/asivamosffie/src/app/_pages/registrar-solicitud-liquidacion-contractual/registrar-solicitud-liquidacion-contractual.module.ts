import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { RegistrarSolicitudLiquidacionContractualRoutingModule } from './registrar-solicitud-liquidacion-contractual-routing.module';
import { HomeComponent } from './components/home/home.component';
import { ExpansionPanelComponent } from './components/expansion-panel/expansion-panel.component';
import { TablaLiquidacionContratoObraComponent } from './components/tabla-liquidacion-contrato-obra/tabla-liquidacion-contrato-obra.component';
import { ValidarRequerimientosLiquidacionesComponent } from './components/validar-requerimientos-liquidaciones/validar-requerimientos-liquidaciones.component';


@NgModule({
  declarations: [HomeComponent, ExpansionPanelComponent, TablaLiquidacionContratoObraComponent, ValidarRequerimientosLiquidacionesComponent],
  imports: [
    CommonModule,
    RegistrarSolicitudLiquidacionContractualRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class RegistrarSolicitudLiquidacionContractualModule { }
