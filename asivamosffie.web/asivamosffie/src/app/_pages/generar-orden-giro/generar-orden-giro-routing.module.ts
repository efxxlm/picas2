import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { FormGenerarOrdenGiroComponent } from "./components/form-generar-orden-giro/form-generar-orden-giro.component";
import { GenerarOrdenGiroComponent } from "./components/generar-orden-giro/generar-orden-giro.component";

const routes: Routes = [
  {
    path: '',
    component: GenerarOrdenGiroComponent
  },
  {
    path: 'generacionOrdenGiro/:id',
    component: FormGenerarOrdenGiroComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GenerarOrdenGiroRoutingModule { }
