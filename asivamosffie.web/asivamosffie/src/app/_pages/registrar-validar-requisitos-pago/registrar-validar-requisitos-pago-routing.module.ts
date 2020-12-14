import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegistrarNuevaSolicitudPagoComponent } from './components/registrar-nueva-solicitud-pago/registrar-nueva-solicitud-pago.component';
import { RegistrarValidarRequisitosPagoComponent } from './components/registrar-validar-requisitos-pago/registrar-validar-requisitos-pago.component';

const routes: Routes = [
  {
    path: '',
    component: RegistrarValidarRequisitosPagoComponent
  },
  {
    path: 'registrarNuevaSolicitudPago',
    component: RegistrarNuevaSolicitudPagoComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistrarValidarRequisitosPagoRoutingModule { }
