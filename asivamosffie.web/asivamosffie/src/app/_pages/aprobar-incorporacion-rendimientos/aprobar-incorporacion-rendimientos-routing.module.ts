import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AprobarIncorporacionRendimientosComponent } from "./components/aprobar-incorporacion-rendimientos/aprobar-incorporacion-rendimientos.component";

const routes: Routes = [
    {
      path: '',
      component: AprobarIncorporacionRendimientosComponent
    }
  ];
  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  
export class AprobarIncorporacionRendimientosRoutingModule { }
