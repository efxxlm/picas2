import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReportesRoutingModule } from './reportes-routing.module';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { HomeComponent } from './component/home/home.component';
import { FichaContratosProyectosComponent } from './component/ficha-contratos-proyectos/ficha-contratos-proyectos.component';
import { TablaResultadosComponent } from './component/tabla-resultados/tabla-resultados.component';
import { MenuFichaComponent } from './component/menu-ficha/menu-ficha.component';
import { MapaInteractivoComponent } from './component/mapa-interactivo/mapa-interactivo.component';
import { ReportesEstandarComponent } from './component/reportes-estandar/reportes-estandar.component';
import { MapaEntidadInstEducativaComponent } from './component/mapa-entidad-inst-educativa/mapa-entidad-inst-educativa.component';
import { MapaEstadisticasContratistasComponent } from './component/mapa-estadisticas-contratistas/mapa-estadisticas-contratistas.component';
import { MapaEstadisticasSupervisoresComponent } from './component/mapa-estadisticas-supervisores/mapa-estadisticas-supervisores.component';


@NgModule({
  declarations: [HomeComponent, FichaContratosProyectosComponent, TablaResultadosComponent, MenuFichaComponent, MapaInteractivoComponent, ReportesEstandarComponent, MapaEntidadInstEducativaComponent, MapaEstadisticasContratistasComponent, MapaEstadisticasSupervisoresComponent],
  imports: [
    CommonModule,
    ReportesRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class ReportesModule { }
