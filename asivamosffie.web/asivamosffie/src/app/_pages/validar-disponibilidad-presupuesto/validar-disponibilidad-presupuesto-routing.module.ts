import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ValidarComponent } from './components/validar/validar.component';
import { ValidacionPresupuestalComponent } from './components/validacion-presupuestal/validacion-presupuestal.component';
import { ConValidacionPresupuestalComponent } from './components/con-validacion-presupuestal/con-validacion-presupuestal.component';
import { DevueltaPorValidacionComponent } from './components/devuelta-por-validacion/devuelta-por-validacion.component';
import { RechasadaPorValidacionComponent } from './components/rechasada-por-validacion/rechasada-por-validacion.component';

const routes: Routes = [
  {
    path: '',
    component: ValidarComponent
  },
  {
    path: 'enValidacionPresupuestal/:id',
    component: ValidacionPresupuestalComponent
  },
  {
    path: 'conValidacionPresupuestal/:id',
    component: ConValidacionPresupuestalComponent
  },
  {
    path: 'devueltaProValidacion/:id',
    component: DevueltaPorValidacionComponent
  },
  {
    path: 'rechazadaProValidacion/:id',
    component: RechasadaPorValidacionComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ValidarDisponibilidadPresupuestoRoutingModule { }
