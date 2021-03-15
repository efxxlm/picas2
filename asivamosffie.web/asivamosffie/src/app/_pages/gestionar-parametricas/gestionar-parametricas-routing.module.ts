import { ConsultarEditarParametricasComponent } from './components/consultar-editar-parametricas/consultar-editar-parametricas.component';
import { FormGestionarParametricasComponent } from './components/form-gestionar-parametricas/form-gestionar-parametricas.component';
import { GestionarParametricasComponent } from './components/gestionar-parametricas/gestionar-parametricas.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [
  {
    path: '',
    component: GestionarParametricasComponent
  },
  {
    path: 'crearValores/:id',
    component: FormGestionarParametricasComponent
  },
  {
    path: 'editarParametricas/:id',
    component: ConsultarEditarParametricasComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarParametricasRoutingModule { }
