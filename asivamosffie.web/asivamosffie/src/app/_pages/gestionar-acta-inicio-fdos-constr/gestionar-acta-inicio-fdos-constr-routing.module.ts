import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GenerarActaFdosConstrComponent } from './components/generar-acta-fdos-constr/generar-acta-fdos-constr.component';
import { GestionarActaInicioFdosConstrComponent } from './components/gestionar-acta-inicio-fdos-constr/gestionar-acta-inicio-fdos-constr.component';


const routes: Routes = [
  {
    path: '',
    component: GestionarActaInicioFdosConstrComponent
  },
  {
    path:'generarActaFDos/:id',
    component: GenerarActaFdosConstrComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarActaInicioFdosConstrRoutingModule { }

