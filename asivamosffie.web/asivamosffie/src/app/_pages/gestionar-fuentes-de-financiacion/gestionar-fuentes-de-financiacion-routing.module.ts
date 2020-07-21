import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BtnRegistrarComponent } from './components/btn-registrar/btn-registrar.component';
import { RegistrarComponent } from './components/registrar/registrar.component';

const routes: Routes = [
  {
    path: '',
    component: BtnRegistrarComponent
  },
  {
    path: 'registrar',
    component: RegistrarComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarFuentesDeFinanciacionRoutingModule { }
