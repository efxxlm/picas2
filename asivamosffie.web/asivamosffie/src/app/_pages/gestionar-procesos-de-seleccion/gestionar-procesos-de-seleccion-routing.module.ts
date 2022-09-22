import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { BtnRegistrarComponent } from './components/btn-registrar/btn-registrar.component';
import { RegistrarNuevoComponent } from './components/registrar-nuevo/registrar-nuevo.component';
import { SeccionPrivadaComponent } from './components/seccion-privada/seccion-privada.component';
import { InvitacionCerradaComponent } from './components/invitacion-cerrada/invitacion-cerrada.component';
import { InvitacionAbiertaComponent } from './components/invitacion-abierta/invitacion-abierta.component';
import { MonitorearCronogramaComponent } from './components/monitorear-cronograma/monitorear-cronograma.component';
import { CurrencyMaskInputMode, NgxCurrencyModule } from "ngx-currency"; 
import { InvitacionInternaComponent } from './components/invitacion-interna/invitacion-interna.component';

export const customCurrencyMaskConfig = {
    align: "right",
    allowNegative: true,
    allowZero: true,
    decimal: ",",
    precision: 0,
    prefix: "$ ",
    suffix: "",
    thousands: ".",
    nullable: true,
    min: null,
    max: null,
    inputMode: CurrencyMaskInputMode.FINANCIAL
};

const routes: Routes = [
  {
    path: '',
    component: BtnRegistrarComponent
  },
  {
    path: 'nuevo',
    component: RegistrarNuevoComponent
  },
  {
    path: 'seccionPrivada/:id',
    component: SeccionPrivadaComponent
  },
  {
    path: 'invitacionCerrada/:id',
    component: InvitacionCerradaComponent
  },
  {
    path: 'invitacionAbierta/:id',
    component: InvitacionAbiertaComponent
  },
  {
    path: 'monitorearCronograma/:id',
    component: MonitorearCronogramaComponent
  },
  {
    path: 'invitacionInterna/:id',
    component: InvitacionInternaComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes),NgxCurrencyModule.forRoot(customCurrencyMaskConfig)],
  exports: [RouterModule]
})
export class GestionarProcesosDeSeleccionRoutingModule { }
