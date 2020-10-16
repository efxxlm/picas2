import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ContratosModificacionesContractualesComponent } from './components/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.component';
import { FormContratacionComponent } from './components/form-contratacion/form-contratacion.component';
import { FormModificacionContractualComponent } from './components/form-modificacion-contractual/form-modificacion-contractual.component';

const routes: Routes = [
  {
    path: '',
    component: ContratosModificacionesContractualesComponent
  },
  {
    path: 'contratacion/:id',
    component: FormContratacionComponent
  },
  {
    path: 'modificacionContractual/:id',
    component: FormModificacionContractualComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ContratosModificacionesContractualesRoutingModule { }
