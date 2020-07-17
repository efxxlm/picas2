import { BotonRegistrarAcuerdoComponent } from './components/boton-registrar-acuerdo/boton-registrar-acuerdo.component';
import { RegistrarAcuerdoComponent } from './components/registrar-acuerdo/registrar-acuerdo.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CurrencyMaskInputMode, NgxCurrencyModule } from "ngx-currency";
 
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
    component: BotonRegistrarAcuerdoComponent
  },
  {
    path: 'resgistrarAcuerdos',
    component: RegistrarAcuerdoComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes),NgxCurrencyModule.forRoot(customCurrencyMaskConfig)],
  exports: [RouterModule]
})
export class GestionarAcuerdoCofinanciacionRoutingModule { }
