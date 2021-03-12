import { FormCrearRolesComponent } from './components/form-crear-roles/form-crear-roles.component';
import { CrearRolesComponent } from './components/crear-roles/crear-roles.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [
  {
    path: '',
    component: CrearRolesComponent
  },
  {
    path: 'nuevoRol',
    component: FormCrearRolesComponent
  },
  {
    path: 'editarRol/:id',
    component: FormCrearRolesComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CrearRolesRoutingModule { }
