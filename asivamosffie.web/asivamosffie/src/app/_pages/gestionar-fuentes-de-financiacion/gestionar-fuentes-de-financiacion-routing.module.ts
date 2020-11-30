import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BtnRegistrarComponent } from './components/btn-registrar/btn-registrar.component';
import { RegistrarComponent } from './components/registrar/registrar.component';
import { ControlDeRecursosComponent } from './components/control-de-recursos/control-de-recursos.component';
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
    component: BtnRegistrarComponent
  },
  {
    path: 'registrar',
    component: RegistrarComponent
  },
  {
    path: 'controlRecursos/:idFuente/:idControl',
    component: ControlDeRecursosComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes),NgxCurrencyModule.forRoot(customCurrencyMaskConfig)],
  exports: [RouterModule]
})
export class GestionarFuentesDeFinanciacionRoutingModule { }
