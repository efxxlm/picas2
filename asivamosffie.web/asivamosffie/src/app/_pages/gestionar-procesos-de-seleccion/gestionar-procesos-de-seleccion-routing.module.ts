import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { BtnRegistrarComponent } from './components/btn-registrar/btn-registrar.component';
import { RegistrarNuevoComponent } from './components/registrar-nuevo/registrar-nuevo.component';
import { SeccionPrivadaComponent } from './components/seccion-privada/seccion-privada.component';
import { InvitacionCerradaComponent } from './components/invitacion-cerrada/invitacion-cerrada.component';
import { InvitacionAbiertaComponent } from './components/invitacion-abierta/invitacion-abierta.component';

const routes: Routes = [
  {
    path: '',
    component: BtnRegistrarComponent
  },
  {
    path: 'nuevo',
    component: RegistrarNuevoComponent
  },
  {
    path: 'seccionPrivada/:id',
    component: SeccionPrivadaComponent
  },
  {
    path: 'invitacionCerrada/:id',
    component: InvitacionCerradaComponent
  },
  {
    path: 'invitacionAbierta/:id',
    component: InvitacionAbiertaComponent
  },
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarProcesosDeSeleccionRoutingModule { }
