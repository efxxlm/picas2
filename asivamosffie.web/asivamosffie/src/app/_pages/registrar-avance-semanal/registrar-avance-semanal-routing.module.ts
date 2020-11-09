import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegistrarAvanceSemanalComponent } from './components/registrar-avance-semanal/registrar-avance-semanal.component';


const routes: Routes = [
  {
    path: '',
    component: RegistrarAvanceSemanalComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistrarAvanceSemanalRoutingModule { }
