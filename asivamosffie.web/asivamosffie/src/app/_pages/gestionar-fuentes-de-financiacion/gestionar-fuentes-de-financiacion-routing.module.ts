import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BtnRegistrarComponent } from './components/btn-registrar/btn-registrar.component';
import { RegistrarComponent } from './components/registrar/registrar.component';
import { ControlDeRecursosComponent } from './components/control-de-recursos/control-de-recursos.component';

const routes: Routes = [
  {
    path: '',
    component: BtnRegistrarComponent
  },
  {
    path: 'registrar',
    component: RegistrarComponent
  },
  {
    path: 'controlRecursos/:idFuente/:idControl',
    component: ControlDeRecursosComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarFuentesDeFinanciacionRoutingModule { }
