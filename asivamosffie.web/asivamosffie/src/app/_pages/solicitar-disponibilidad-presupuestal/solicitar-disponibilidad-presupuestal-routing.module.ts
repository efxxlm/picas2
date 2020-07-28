import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TituloComponent } from './components/titulo/titulo.component';
import { TablaCrearSolicitudTradicionalComponent } from './components/tabla-crear-solicitud-tradicional/tabla-crear-solicitud-tradicional.component';

const routes: Routes = [
  {
    path: '',
    component: TituloComponent
  },
  {
    path: 'crear-solicitud-tradicional',
    component: TablaCrearSolicitudTradicionalComponent
  },
  {
    path: 'crear-solicitud-especial',
    component: TituloComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SolicitarDisponibilidadPresupuestalRoutingModule { }
