import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ComiteTecnicoComponent } from './components/comite-tecnico/comite-tecnico.component';
import { CrearOrdenDelDiaComponent } from './components/crear-orden-del-dia/crear-orden-del-dia.component';
import { RegistrarSesionComiteTecnicoComponent } from './components/registrar-sesion-comite-tecnico/registrar-sesion-comite-tecnico.component';
import { FormRegistrarParticipantesComponent } from './components/form-registrar-participantes/form-registrar-participantes.component';

const routes: Routes = [
  {
    path: '',
    component: ComiteTecnicoComponent
  },
  {
    path: 'crearOrdenDelDia/:id/:fecha',
    component: CrearOrdenDelDiaComponent
  },
  {
    path: 'registrarSesionDeComiteTecnico/:id',
    component: RegistrarSesionComiteTecnicoComponent
  },
  {
    path: 'registrarSesionDeComiteTecnico/:id/registrarParticipantes',
    component: FormRegistrarParticipantesComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ComiteTecnicoRoutingModule { }
