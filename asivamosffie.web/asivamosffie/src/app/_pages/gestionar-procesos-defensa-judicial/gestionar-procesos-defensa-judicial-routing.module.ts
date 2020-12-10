import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GestionarProcesosDefensaJudicialComponent } from './components/gestionar-procesos-defensa-judicial/gestionar-procesos-defensa-judicial.component';


const routes: Routes = [
  {
    path: '',
    component: GestionarProcesosDefensaJudicialComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarProcesosDefensaJudicialRoutingModule { }