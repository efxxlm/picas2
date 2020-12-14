import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegistrarValidarRequisitosPagoComponent } from './components/registrar-validar-requisitos-pago/registrar-validar-requisitos-pago.component';

const routes: Routes = [
  {
    path: '',
    component: RegistrarValidarRequisitosPagoComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistrarValidarRequisitosPagoRoutingModule { }
