import { CambiarContrasenaComponent } from './components/cambiar-contrasena/cambiar-contrasena.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [
  {
    path: '',
    component: CambiarContrasenaComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CambiarContrasenaRoutingModule { }
