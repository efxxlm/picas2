import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MenuComponent } from './components/menu/menu.component';
import { GestionarPolizasComponent } from './components/gestionar-polizas/gestionar-polizas.component';
import { EditarEnRevisionComponent } from './components/editar-en-revision/editar-en-revision.component';


const routes: Routes = [
  {
    path: '',
    component: MenuComponent
  },
  {
    path: 'gestionar-polizas/:id',
    component: GestionarPolizasComponent
  },
  {
    path: 'enRevision/:id',
    component: EditarEnRevisionComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GenerarPolizasYGarantiasRoutingModule { }
