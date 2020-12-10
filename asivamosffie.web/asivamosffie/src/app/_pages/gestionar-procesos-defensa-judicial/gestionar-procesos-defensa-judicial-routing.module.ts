import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GestionarProcesosDefensaJudicialComponent } from './components/gestionar-procesos-defensa-judicial/gestionar-procesos-defensa-judicial.component';
import { RegistroNuevoProcesoJudicialComponent } from './components/registro-nuevo-proceso-judicial/registro-nuevo-proceso-judicial.component';


const routes: Routes = [
  {
    path: '',
    component: GestionarProcesosDefensaJudicialComponent
  },
  {
    path: 'registrarNuevoProcesoJudicial',
    component: RegistroNuevoProcesoJudicialComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarProcesosDefensaJudicialRoutingModule { }