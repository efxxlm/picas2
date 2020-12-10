import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CurrencyMaskInputMode, NgxCurrencyModule } from 'ngx-currency';
import { GestionarProcesosDefensaJudicialComponent } from './components/gestionar-procesos-defensa-judicial/gestionar-procesos-defensa-judicial.component';
import { RegistroNuevoProcesoJudicialComponent } from './components/registro-nuevo-proceso-judicial/registro-nuevo-proceso-judicial.component';
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
    component: GestionarProcesosDefensaJudicialComponent
  },
  {
    path: 'registrarNuevoProcesoJudicial',
    component: RegistroNuevoProcesoJudicialComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes),NgxCurrencyModule.forRoot(customCurrencyMaskConfig)],
  exports: [RouterModule]
})
export class GestionarProcesosDefensaJudicialRoutingModule { }