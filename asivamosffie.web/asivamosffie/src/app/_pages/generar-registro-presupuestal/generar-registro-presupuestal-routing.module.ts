import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GenerarRegistroPresupuestalComponent } from './components/generar-registro-presupuestal/generar-registro-presupuestal.component';
import { GestionarDrpComponent } from './components/gestionar-drp/gestionar-drp.component';
import { VerDetalleRegistroPresupuestalComponent } from './components/ver-detalle-registro-presupuestal/ver-detalle-registro-presupuestal.component';

const routes: Routes = [
  {
    path: '',
    component: GenerarRegistroPresupuestalComponent
  },
  {
    path: 'verDetalle/:id',
    component: VerDetalleRegistroPresupuestalComponent
  },
  {
    path: 'gestionarDrp/:id',
    component: GestionarDrpComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})


export class GenerarRegistroPresupuestalRoutingModule { }
