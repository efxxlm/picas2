import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SolicitarContratacionComponent } from './components/solicitar-contratacion/solicitar-contratacion.component';
import { FormSolicitarContratacionComponent } from './components/form-solicitar-contratacion/form-solicitar-contratacion.component';

const routes: Routes = [
  {
    path: '',
    component: SolicitarContratacionComponent
  },
  {
    path: 'solicitar',
    component: FormSolicitarContratacionComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SolicitarContratacionRoutingModule { }
