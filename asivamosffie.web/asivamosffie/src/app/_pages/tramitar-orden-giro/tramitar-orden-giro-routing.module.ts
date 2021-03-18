import { FormTramitarOrdenGiroComponent } from './components/form-tramitar-orden-giro/form-tramitar-orden-giro.component';
import { TramitarOrdenGiroComponent } from './components/tramitar-orden-giro/tramitar-orden-giro.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [
  {
    path: '',
    component: TramitarOrdenGiroComponent
  },
  {
    path: 'tramitarOrdenGiro/:id',
    component: FormTramitarOrdenGiroComponent
  },
  {
    path: 'editarOrdenGiro/:id',
    component: FormTramitarOrdenGiroComponent
  },
  {
    path: 'verDetalle/:id',
    component: FormTramitarOrdenGiroComponent
  },
  {
    path: 'tramitarOrdenGiroExpensas/:id',
    component: FormTramitarOrdenGiroComponent
  },
  {
    path: 'editarOrdenGiroExpensas/:id',
    component: FormTramitarOrdenGiroComponent
  },
  {
    path: 'verDetalleExpensas/:id',
    component: FormTramitarOrdenGiroComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TramitarOrdenGiroRoutingModule { }
