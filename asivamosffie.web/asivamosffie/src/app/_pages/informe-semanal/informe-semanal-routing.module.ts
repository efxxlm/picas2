import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InformeSemanalComponent } from './components/informe-semanal/informe-semanal.component';


const routes: Routes = [
  {
    path: '',
    component: InformeSemanalComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InformeSemanalRoutingModule { }
