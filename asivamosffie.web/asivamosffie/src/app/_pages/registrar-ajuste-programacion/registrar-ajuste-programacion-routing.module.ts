import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { RegistrarAjusteProgramacionComponent } from './components/registrar-ajuste-programacion/registrar-ajuste-programacion.component';
import { VerHistorialComponent } from './components/ver-historial/ver-historial.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'registrarAjusteProgramacion/:id',
    component: RegistrarAjusteProgramacionComponent
  },
  {
    path: 'verDetalleEditarAjusteProgramacion/:id',
    component: RegistrarAjusteProgramacionComponent
  },
  {
    path: 'verDetalleAjusteProgramacion/:id',
    component: RegistrarAjusteProgramacionComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistrarAjusteProgramacionRoutingModule { }
