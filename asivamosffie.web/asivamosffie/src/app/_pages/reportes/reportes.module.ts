import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReportesRoutingModule } from './reportes-routing.module';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { HomeComponent } from './component/home/home.component';
import { FichaContratosProyectosComponent } from './component/proyecto/ficha-contratos-proyectos/ficha-contratos-proyectos.component';
import { TablaResultadosComponent } from './component/contrato/tabla-resultados/tabla-resultados.component';
import { MenuFichaComponent } from './component/contrato/menu-ficha/menu-ficha.component';
import { MapaInteractivoComponent } from './component/mapa-interactivo/mapa-interactivo.component';
import { ReportesEstandarComponent } from './component/reportes-estandar/reportes-estandar.component';
import { MapaEntidadInstEducativaComponent } from './component/mapa-entidad-inst-educativa/mapa-entidad-inst-educativa.component';
import { MapaEstadisticasContratistasComponent } from './component/mapa-estadisticas-contratistas/mapa-estadisticas-contratistas.component';
import { MapaEstadisticasSupervisoresComponent } from './component/mapa-estadisticas-supervisores/mapa-estadisticas-supervisores.component';
import { ResumenComponent } from './component/contrato/resumen/resumen.component';
import { SeleccionComponent } from './component/contrato/seleccion/seleccion.component';
import { ContratacionComponent } from './component/contrato/contratacion/contratacion.component';
import { PolizasSegurosComponent } from './component/contrato/polizas-seguros/polizas-seguros.component';
import { EjecucionFinancieraComponent } from './component/contrato/ejecucion-financiera/ejecucion-financiera.component';
import { NovedadesComponent } from './component/contrato/novedades/novedades.component';
import { ControversiasComponent } from './component/contrato/controversias/controversias.component';
import { ProcesosDefensaJudicialComponent } from './component/contrato/procesos-defensa-judicial/procesos-defensa-judicial.component';
import { LiquidacionComponent } from './component/contrato/liquidacion/liquidacion.component';
import { FichaContratoComponent } from './component/contrato/ficha-contrato/ficha-contrato.component';
import { FichaProyectoComponent } from './component/proyecto/ficha-proyecto/ficha-proyecto.component';
import { TablaResultadosProyectoComponent } from './component/proyecto/tabla-resultados-proyecto/tabla-resultados-proyecto.component';
import { MenuFichaProyectoComponent } from './component/proyecto/menu-ficha-proyecto/menu-ficha-proyecto.component';
import { ResumenPComponent } from './component/proyecto/resumen-p/resumen-p.component';
import { ContratacionPComponent } from './component/proyecto/contratacion-p/contratacion-p.component';
import { PreparacionComponent } from './component/proyecto/preparacion/preparacion.component';
import { SeguimientoTecnicoComponent } from './component/proyecto/seguimiento-tecnico/seguimiento-tecnico.component';
import { SeguimientoFinancieroComponent } from './component/proyecto/seguimiento-financiero/seguimiento-financiero.component';
import { EntregaComponent } from './component/proyecto/entrega/entrega.component';
import { EmbeddedPowerBiComponent } from './component/embedded-power-bi/embedded-power-bi.component';

@NgModule({
  declarations: [
    HomeComponent,
    FichaContratosProyectosComponent,
    TablaResultadosComponent,
    MenuFichaComponent,
    MapaInteractivoComponent,
    ReportesEstandarComponent,
    MapaEntidadInstEducativaComponent,
    MapaEstadisticasContratistasComponent,
    MapaEstadisticasSupervisoresComponent,
    ResumenComponent,
    SeleccionComponent,
    ContratacionComponent,
    PolizasSegurosComponent,
    EjecucionFinancieraComponent,
    NovedadesComponent,
    ControversiasComponent,
    ProcesosDefensaJudicialComponent,
    LiquidacionComponent,
    FichaContratoComponent,
    FichaProyectoComponent,
    TablaResultadosProyectoComponent,
    MenuFichaProyectoComponent,
    ResumenPComponent,
    ContratacionPComponent,
    PreparacionComponent,
    SeguimientoTecnicoComponent,
    SeguimientoFinancieroComponent,
    EntregaComponent,
    EmbeddedPowerBiComponent
  ],
  imports: [
    CommonModule,
    ReportesRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class ReportesModule {}
