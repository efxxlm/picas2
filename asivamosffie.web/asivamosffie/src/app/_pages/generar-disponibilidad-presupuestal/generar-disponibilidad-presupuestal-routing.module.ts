import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MenuGenerarDisponibilidadComponent } from './components/menu-generar-disponibilidad/menu-generar-disponibilidad.component';

const routes: Routes = [
  {
    path: '',
    component: MenuGenerarDisponibilidadComponent
  },
  {
    path: 'DDP/:id',
    component: MenuGenerarDisponibilidadComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GenerarDisponibilidadPresupuestalRoutingModule { }
