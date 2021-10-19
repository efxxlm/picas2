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
    path: 'DDP/:id/:esNovedad/:novedadId',
    component: GestionarDdpComponent
  },
  {
    path: 'detalleConDisponibilidadPresupuestal/:id/:esNovedad/:novedadId',
    component: DetalleConValidacionPresupuestalComponent
  },
  {
    path: 'conLiberacionSaldo/:id/:esNovedad/:novedadId',
    component: DetalleConValidacionPresupuestalComponent
  },
  {
    path: 'detalleConDisponibilidadCancelada/:id/:esNovedad/:novedadId',
    component: DetalleConValidacionPresupuestalComponent
  },
  {
    path: 'devueltaPorCoordinacionFinanciera/:id/:esNovedad/:novedadId',
    component: DevueltaPorCoordinacionFinancieraComponent
  },
  {
    path: 'detalleConDisponibilidadCancelada/:id',
    component: DetalleConDisponibilidadCanceladaComponent
  },
  {
    path: 'detalleRechazadaFiduciaria/:id/:esNovedad/:novedadId',
    component: DetalleConValidacionPresupuestalComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GenerarDisponibilidadPresupuestalRoutingModule { }
