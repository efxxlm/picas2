import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ValidarComponent } from './components/validar/validar.component';
import { ValidacionPresupuestalComponent } from './components/validacion-presupuestal/validacion-presupuestal.component';

const routes: Routes = [
  {
    path: '',
    component: ValidarComponent
  },
  {
    path: 'validacionPresupuestal/:id',
    component: ValidacionPresupuestalComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ValidarDisponibilidadPresupuestoRoutingModule { }
