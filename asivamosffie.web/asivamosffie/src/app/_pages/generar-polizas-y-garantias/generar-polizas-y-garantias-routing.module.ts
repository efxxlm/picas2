import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MenuComponent } from './components/menu/menu.component';
import { GestionarPolizasComponent } from './components/gestionar-polizas/gestionar-polizas.component';


const routes: Routes = [
  {
    path: '',
    component: MenuComponent
  },
  {
    path: 'gestionar-polizas/:id',
    component: GestionarPolizasComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GenerarPolizasYGarantiasRoutingModule { }
