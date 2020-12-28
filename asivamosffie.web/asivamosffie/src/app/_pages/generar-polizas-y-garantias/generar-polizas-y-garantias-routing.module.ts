import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MenuComponent } from './components/menu/menu.component';
import { GestionarPolizasComponent } from './components/gestionar-polizas/gestionar-polizas.component';
import { EditarEnRevisionComponent } from './components/editar-en-revision/editar-en-revision.component';
import { EditarObservadaODevueltaComponent } from './components/editar-observada-o-devuelta/editar-observada-o-devuelta.component';
import { VerDetallePolizaComponent } from './components/ver-detalle-poliza/ver-detalle-poliza.component';


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
  },
  {
    path: 'observadaODevuelta/:id',
    component: EditarObservadaODevueltaComponent
  },
  {
    path: 'verDetalle/:id',
    component: VerDetallePolizaComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GenerarPolizasYGarantiasRoutingModule { }
