import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './component/home/home.component';
import { FichaContratosProyectosComponent } from './component/ficha-contratos-proyectos/ficha-contratos-proyectos.component';
import { MapaInteractivoComponent } from './component/mapa-interactivo/mapa-interactivo.component';
import { ReportesEstandarComponent } from './component/reportes-estandar/reportes-estandar.component';

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
