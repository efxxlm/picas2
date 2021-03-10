import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { ValidarInformeComponent } from './components/validar-informe/validar-informe.component';
import { VerDetalleInformeFinalComponent } from './components/ver-detalle-informe-final/ver-detalle-informe-final.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'validar/:id',
    component: ValidarInformeComponent
  },
  {
    path: 'verDetalle/:id',
    component: VerDetalleInformeFinalComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ValidarInformeDelProyectoRoutingModule { }
