import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ValidarComponent } from './components/validar/validar.component';
import { ValidacionPresupuestalComponent } from './components/validacion-presupuestal/validacion-presupuestal.component';
import { ConValidacionPresupuestalComponent } from './components/con-validacion-presupuestal/con-validacion-presupuestal.component';
import { ConDisponibilidadComponent } from './components/con-disponibilidad/con-disponibilidad.component';
import { DevueltaPorValidacionComponent } from './components/devuelta-por-validacion/devuelta-por-validacion.component';
import { DevueltaPorCoordinacionComponent } from './components/devuelta-por-coordinacion/devuelta-por-coordinacion.component';
import { RechasadaPorValidacionComponent } from './components/rechasada-por-validacion/rechasada-por-validacion.component';
import { ConDisponibilidadCanceladaComponent } from './components/con-disponibilidad-cancelada/con-disponibilidad-cancelada.component';

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
    path: 'conDisponibilidadPresupuestal/:id',
    component: ConDisponibilidadComponent
  },
  {
    path: 'devueltaProValidacion/:id',
    component: DevueltaPorValidacionComponent
  },
  {
    path: 'devueltaProCoordinacionFinanciera/:id',
    component: DevueltaPorCoordinacionComponent
  },
  {
    path: 'rechazadaProValidacion/:id',
    component: RechasadaPorValidacionComponent
  },
  {
    path: 'conDisponibilidadcancelada/:id',
    component: ConDisponibilidadCanceladaComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ValidarDisponibilidadPresupuestoRoutingModule { }
