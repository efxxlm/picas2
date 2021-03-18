import { FormGestionarUsuariosComponent } from './components/form-gestionar-usuarios/form-gestionar-usuarios.component';
import { GestionarUsuariosComponent } from './components/gestionar-usuarios/gestionar-usuarios.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [
  {
    path: '',
    component: GestionarUsuariosComponent
  },
  {
    path: 'crearUsuario',
    component: FormGestionarUsuariosComponent
  },
  {
    path: 'editarUsuario/:id',
    component: FormGestionarUsuariosComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarUsuariosRoutingModule { }
