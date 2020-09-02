import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProcesosContractualesComponent } from './components/procesos-contractuales/procesos-contractuales.component';
import { FormContratacionComponent } from './components/form-contratacion/form-contratacion.component';
import { FormModificacionContractualComponent } from './components/form-modificacion-contractual/form-modificacion-contractual.component';
import { FormLiquidacionComponent } from './components/form-liquidacion/form-liquidacion.component';


const routes: Routes = [
  {
    path: '',
    component: ProcesosContractualesComponent
  },
  {
    path: 'contratacion/:id',
    component: FormContratacionComponent
  },
  {
    path: 'modificacionContractual/:id',
    component: FormModificacionContractualComponent
  },
  {
    path: 'liquidacion/:id',
    component: FormLiquidacionComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarProcesosContractualesRoutingModule { }
