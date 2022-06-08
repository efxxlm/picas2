import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './component/home/home.component';
import { FichaContratosProyectosComponent } from './component/proyecto/ficha-contratos-proyectos/ficha-contratos-proyectos.component';
import { MapaInteractivoComponent } from './component/mapa-interactivo/mapa-interactivo.component';
import { ReportesEstandarComponent } from './component/reportes-estandar/reportes-estandar.component';
import { ResumenComponent } from './component/contrato/resumen/resumen.component';
import { SeleccionComponent } from './component/contrato/seleccion/seleccion.component';
import { ContratacionComponent } from './component/contrato/contratacion/contratacion.component';
import { PolizasSegurosComponent } from './component/contrato/polizas-seguros/polizas-seguros.component';
import { EjecucionFinancieraComponent } from './component/contrato/ejecucion-financiera/ejecucion-financiera.component';
import { NovedadesComponent } from './component/contrato/novedades/novedades.component';
import { ControversiasComponent } from './component/contrato/controversias/controversias.component';
import { ProcesosDefensaJudicialComponent } from './component/contrato/procesos-defensa-judicial/procesos-defensa-judicial.component';
import { LiquidacionComponent } from './component/contrato/liquidacion/liquidacion.component';
import { ResumenPComponent } from './component/proyecto/resumen-p/resumen-p.component';
import { ContratacionPComponent } from './component/proyecto/contratacion-p/contratacion-p.component';
import { PreparacionComponent } from './component/proyecto/preparacion/preparacion.component';
import { SeguimientoTecnicoComponent } from './component/proyecto/seguimiento-tecnico/seguimiento-tecnico.component';
import { SeguimientoFinancieroComponent } from './component/proyecto/seguimiento-financiero/seguimiento-financiero.component';
import { EntregaComponent } from './component/proyecto/entrega/entrega.component';
import { EmbeddedPowerBiComponent } from './component/embedded-power-bi/embedded-power-bi.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'fichaContratosProyectos',
    component: FichaContratosProyectosComponent
  },
  {
    path: 'fichaContratosProyectos/fichaContrato/resumen/:id',
    component: ResumenComponent
  },
  {
    path: 'fichaContratosProyectos/fichaContrato/seleccion/:id',
    component: SeleccionComponent
  },
  {
    path: 'fichaContratosProyectos/fichaContrato/contratacion/:id',
    component: ContratacionComponent
  },
  {
    path: 'fichaContratosProyectos/fichaContrato/polizasSeguros/:id',
    component: PolizasSegurosComponent
  },
  {
    path: 'fichaContratosProyectos/fichaContrato/ejecucionFinanciera/:id',
    component: EjecucionFinancieraComponent
  },
  {
    path: 'fichaContratosProyectos/fichaContrato/novedades/:id',
    component: NovedadesComponent
  },
  {
    path: 'fichaContratosProyectos/fichaContrato/controversias/:id',
    component: ControversiasComponent
  },
  {
    path: 'fichaContratosProyectos/fichaContrato/procesosDefensaJudicial/:id',
    component: ProcesosDefensaJudicialComponent
  },
  {
    path: 'fichaContratosProyectos/fichaContrato/liquidacion/:id',
    component: LiquidacionComponent
  },
  {
    path: 'fichaContratosProyectos/fichaProyecto/resumen/:id',
    component: ResumenPComponent
  },
  {
    path: 'fichaContratosProyectos/fichaProyecto/contratacion/:id',
    component: ContratacionPComponent
  },
  {
    path: 'fichaContratosProyectos/fichaProyecto/preparacion/:id',
    component: PreparacionComponent
  },
  {
    path: 'fichaContratosProyectos/fichaProyecto/seguimientoTecnico/:id',
    component: SeguimientoTecnicoComponent
  },
  {
    path: 'fichaContratosProyectos/fichaProyecto/seguimientoFinanciero/:id',
    component: SeguimientoFinancieroComponent
  },
  {
    path: 'fichaContratosProyectos/fichaProyecto/entrega/:id',
    component: EntregaComponent
  },
  {
    path: 'mapaInteractivo',
    component: MapaInteractivoComponent
  },
  {
    path: 'mapaInteractivo/:id',
    component: EmbeddedPowerBiComponent
  },
  {
    path: 'reportesEstandar',
    component: ReportesEstandarComponent
  },
  {
    path: 'reportesEstandar/:id',
    component: EmbeddedPowerBiComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportesRoutingModule {}
