import { FormVerificarOrdenGiroComponent } from './components/form-verificar-orden-giro/form-verificar-orden-giro.component';
import { VerificarOrdenGiroComponent } from './components/verificar-orden-giro/verificar-orden-giro.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [
  {
    path: '',
    component: VerificarOrdenGiroComponent
  },
  {
    path: 'verificarOrdenGiro/:id',
    component: FormVerificarOrdenGiroComponent
  },
  {
    path: 'editarOrdenGiro/:id',
    component: FormVerificarOrdenGiroComponent
  },
  {
    path: 'verDetalle/:id',
    component: FormVerificarOrdenGiroComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VerificarOrdenGiroRoutingModule { }
