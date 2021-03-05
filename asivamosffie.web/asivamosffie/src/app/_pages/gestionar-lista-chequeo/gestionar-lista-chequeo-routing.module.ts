import { FormCrearBancoComponent } from './components/form-crear-banco/form-crear-banco.component';
import { GestionarListaChequeoComponent } from './components/gestionar-lista-chequeo/gestionar-lista-chequeo.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FormListaChequeoComponent } from './components/form-lista-chequeo/form-lista-chequeo.component';


const routes: Routes = [
  {
    path: '',
    component: GestionarListaChequeoComponent
  },
  {
    path: 'crearBanco',
    component: FormCrearBancoComponent
  },
  {
    path: 'editarBanco/:id',
    component: FormCrearBancoComponent
  },
  {
    path: 'crearListaChequeo',
    component: FormListaChequeoComponent
  },
  {
    path: 'editarListaChequeo/:id',
    component: FormListaChequeoComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarListaChequeoRoutingModule { }
