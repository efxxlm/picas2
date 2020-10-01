import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GestionarActaInicioFdosConstrComponent } from './components/gestionar-acta-inicio-fdos-constr/gestionar-acta-inicio-fdos-constr.component';


const routes: Routes = [
  {
    path: '',
    component: GestionarActaInicioFdosConstrComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarActaInicioFdosConstrRoutingModule { }

