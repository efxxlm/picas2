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
    path: 'fichaContratosProyectos/fichaContrato/resumen',
    component: ResumenComponent
  },
  {
    path: 'fichaContratosProyectos/fichaContrato/seleccion',
    component: SeleccionComponent
  },
  {
    path: 'fichaContratosProyectos/fichaContrato/contratacion',
    component: ContratacionComponent
  },
  {
    path: 'fichaContratosProyectos/fichaContrato/polizasSeguros',
    component: PolizasSegurosComponent
  },
  {
    path: 'fichaContratosProyectos/fichaContrato/ejecucionFinanciera',
    component: EjecucionFinancieraComponent
  },
  {
    path: 'fichaContratosProyectos/fichaContrato/novedades',
    component: NovedadesComponent
  },
  {
    path: 'fichaContratosProyectos/fichaContrato/controversias',
    component: ControversiasComponent
  },
  {
    path: 'fichaContratosProyectos/fichaContrato/procesosDefensaJudicial',
    component: ProcesosDefensaJudicialComponent
  },
  {
    path: 'fichaContratosProyectos/fichaContrato/liquidacion',
    component: LiquidacionComponent
  },
  {
    path: 'fichaContratosProyectos/fichaProyecto/resumen',
    component: ResumenPComponent
  },
  {
    path: 'fichaContratosProyectos/fichaProyecto/contratacion',
    component: ContratacionPComponent
  },
  {
    path: 'fichaContratosProyectos/fichaProyecto/contratacion',
    component: ContratacionPComponent
  },
  {
    path: 'fichaContratosProyectos/fichaProyecto/preparacion',
    component: PreparacionComponent
  },
  {
    path: 'fichaContratosProyectos/fichaProyecto/seguimientoTecnico',
    component: SeguimientoTecnicoComponent
  },
  {
    path: 'fichaContratosProyectos/fichaProyecto/seguimientoFinanciero',
    component: SeguimientoFinancieroComponent
  },
  {
    path: 'fichaContratosProyectos/fichaProyecto/entrega',
    component: EntregaComponent
  },
  {
    path: 'mapaInteractivo',
    component: MapaInteractivoComponent
  },
  {
    path: 'reportesEstandar',
    component: ReportesEstandarComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportesRoutingModule {}
