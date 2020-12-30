import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { GenerarOrdenGiroComponent } from "./components/generar-orden-giro/generar-orden-giro.component";

const routes: Routes = [
  {
    path: '',
    component: GenerarOrdenGiroComponent
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GenerarOrdenGiroRoutingModule { }
