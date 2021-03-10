import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { RevisarInformeComponent } from './components/revisar-informe/revisar-informe.component';
import { DetalleInformeComponent } from './components/detalle-informe/detalle-informe.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'revisarInforme/:id',
    component: RevisarInformeComponent
  },
  {
    path: 'verDetalle/:id',
    component: DetalleInformeComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ValidarCumplimientoInformeFinalProyectoRoutingModule { }
