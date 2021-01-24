import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { ValidarAjusteComponent } from './components/validar-ajuste/validar-ajuste.component';
import { HistorialComponent } from './components/historial/historial.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'validar/:id',
    component: ValidarAjusteComponent
  },
  {
    path: 'historial/:id',
    component: HistorialComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ValidarAjusteProgramacionRoutingModule { }
