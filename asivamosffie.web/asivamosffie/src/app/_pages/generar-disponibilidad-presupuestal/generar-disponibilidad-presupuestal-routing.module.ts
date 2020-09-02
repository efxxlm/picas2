import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MenuGenerarDisponibilidadComponent } from './components/menu-generar-disponibilidad/menu-generar-disponibilidad.component';
import { GestionarDdpComponent } from './components/gestionar-ddp/gestionar-ddp.component';
import { DetalleConValidacionPresupuestalComponent } from './components/detalle-con-validacion-presupuestal/detalle-con-validacion-presupuestal.component';
import { DevueltaPorCoordinacionFinancieraComponent } from './components/devuelta-por-coordinacion-financiera/devuelta-por-coordinacion-financiera.component';
import { DetalleConDisponibilidadCanceladaComponent } from './components/detalle-con-disponibilidad-cancelada/detalle-con-disponibilidad-cancelada.component';

const routes: Routes = [
  {
    path: '',
    component: MenuGenerarDisponibilidadComponent
  },
  {
    path: 'DDP/:id',
    component: GestionarDdpComponent
  },
  {
    path: 'detalleConDisponibilidadPresupuestal/:id',
    component: DetalleConValidacionPresupuestalComponent
  },
  {
    path: 'devueltaPorCoordinacionFinanciera/:id',
    component: DevueltaPorCoordinacionFinancieraComponent
  },
  {
    path: 'detalleConDisponibilidadCancelada/:id',
    component: DetalleConDisponibilidadCanceladaComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GenerarDisponibilidadPresupuestalRoutingModule { }
