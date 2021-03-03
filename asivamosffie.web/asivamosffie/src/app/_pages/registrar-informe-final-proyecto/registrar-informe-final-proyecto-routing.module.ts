import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { RegistrarInformeFinalComponent } from './components/registrar-informe-final/registrar-informe-final.component';
import { VerDetalleInformeFinalComponent } from './components/ver-detalle-informe-final/ver-detalle-informe-final.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'registrar/:id',
    component: RegistrarInformeFinalComponent
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
export class RegistrarInformeFinalProyectoRoutingModule { }
