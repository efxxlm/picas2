import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProcesosContractualesComponent } from './components/procesos-contractuales/procesos-contractuales.component';
import { FormContratacionComponent } from './components/form-contratacion/form-contratacion.component';


const routes: Routes = [
  {
    path: '',
    component: ProcesosContractualesComponent
  },
  {
    path: 'contratacion/:id',
    component: FormContratacionComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarProcesosContractualesRoutingModule { }
