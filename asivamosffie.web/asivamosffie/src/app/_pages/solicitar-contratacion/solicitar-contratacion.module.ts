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
import { DialogTableProyectosSeleccionadosComponent } from './components/dialog-table-proyectos-seleccionados/dialog-table-proyectos-seleccionados.component';
import { TableSolicitudContratacionComponent } from './components/table-solicitud-contratacion/table-solicitud-contratacion.component';
import { ExpansionPanelDetallarSolicitudComponent } from './components/expansion-panel-detallar-solicitud/expansion-panel-detallar-solicitud.component';
import { TableProyectosDeLaSolicitudComponent } from './components/table-proyectos-de-la-solicitud/table-proyectos-de-la-solicitud.component';
import { TablaResultadosContratistasComponent } from './components/tabla-resultados-contratistas/tabla-resultados-contratistas.component';
import { TableCaracteristicasEspecialesComponent } from './components/table-caracteristicas-especiales/table-caracteristicas-especiales.component';
import { DefinirCaracteristicasComponent } from './components/definir-caracteristicas/definir-caracteristicas.component';
import { ConsideracionesEspecialesComponent } from './components/consideraciones-especiales/consideraciones-especiales.component';
import { TableFuentesYUsosComponent } from './components/table-fuentes-y-usos/table-fuentes-y-usos.component';
import { DefinirFuentesYUsosComponent } from './components/definir-fuentes-y-usos/definir-fuentes-y-usos.component';


@NgModule({
  declarations: [SolicitarContratacionComponent, FormSolicitarContratacionComponent, TablaResultadosComponent, DialogTableProyectosSeleccionadosComponent, TableSolicitudContratacionComponent, ExpansionPanelDetallarSolicitudComponent, TableProyectosDeLaSolicitudComponent, TablaResultadosContratistasComponent, TableCaracteristicasEspecialesComponent, DefinirCaracteristicasComponent, ConsideracionesEspecialesComponent, TableFuentesYUsosComponent, DefinirFuentesYUsosComponent],
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
