import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FormularioProyectosComponent } from './components/formulario-proyectos/formulario-proyectos.component';
import { CrearProyectoAdminModule } from './crear-proyecto-admin.module';
import { CrearProyectoAdminComponent } from './components/crear-proyecto-admin/crear-proyecto-admin.component';
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


const routes: Routes = [{
  path: 'crearProyecto',
  component: FormularioProyectosComponent
},
{
  path: '',
  component: CrearProyectoAdminComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes),NgxCurrencyModule.forRoot(customCurrencyMaskConfig)],
  exports: [RouterModule]
})
export class CrearProyectoAdminRoutingModule { }
