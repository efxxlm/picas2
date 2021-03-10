import { FormAprobarOrdenGiroComponent } from './components/form-aprobar-orden-giro/form-aprobar-orden-giro.component';
import { AprobarOrdenGiroComponent } from './components/aprobar-orden-giro/aprobar-orden-giro.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [
  {
    path: '',
    component: AprobarOrdenGiroComponent
  },
  {
    path: 'aprobarOrdenGiro/:id',
    component: FormAprobarOrdenGiroComponent
  },
  {
    path: 'editarOrdenGiro/:id',
    component: FormAprobarOrdenGiroComponent
  },
  {
    path: 'verDetalle/:id',
    component: FormAprobarOrdenGiroComponent
  },
  {
    path: 'aprobarOrdenGiroExpensas/:id',
    component: FormAprobarOrdenGiroComponent
  },
  {
    path: 'editarOrdenGiroExpensas/:id',
    component: FormAprobarOrdenGiroComponent
  },
  {
    path: 'verDetalleExpensas/:id',
    component: FormAprobarOrdenGiroComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AprobarOrdenGiroRoutingModule { }
