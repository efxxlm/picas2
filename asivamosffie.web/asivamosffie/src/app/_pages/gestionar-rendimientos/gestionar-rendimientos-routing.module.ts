import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { GestionarRendimientosComponent } from "./components/gestionar-rendimientos/gestionar-rendimientos.component";

const routes: Routes = [
  {
    path: '',
    component: GestionarRendimientosComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class GestionarRendimientosRoutingModule { }