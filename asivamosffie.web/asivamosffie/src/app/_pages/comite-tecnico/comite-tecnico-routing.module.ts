import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ComiteTecnicoComponent } from './components/comite-tecnico/comite-tecnico.component';
import { CrearOrdenDelDiaComponent } from './components/crear-orden-del-dia/crear-orden-del-dia.component';

const routes: Routes = [
  {
    path: '',
    component: ComiteTecnicoComponent
  },
  {
    path: 'crearOrdenDelDia',
    component: CrearOrdenDelDiaComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ComiteTecnicoRoutingModule { }
