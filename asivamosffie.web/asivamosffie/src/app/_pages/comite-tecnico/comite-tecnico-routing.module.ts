import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ComiteTecnicoComponent } from './components/comite-tecnico/comite-tecnico.component';
import { CrearOrdenDelDiaComponent } from './components/crear-orden-del-dia/crear-orden-del-dia.component';
import { RegistrarSesionComiteTecnicoComponent } from './components/registrar-sesion-comite-tecnico/registrar-sesion-comite-tecnico.component';
import { FormRegistrarParticipantesComponent } from './components/form-registrar-participantes/form-registrar-participantes.component';
import { ObservacionComponent } from './components/observacion/observacion.component';
import { CrearActaComponent } from './components/crear-acta/crear-acta.component';
import { TablaVerificarCumplimientoComponent } from './components/tabla-verificar-cumplimiento/tabla-verificar-cumplimiento.component';

const routes: Routes = [
  {
    path: '',
    component: ComiteTecnicoComponent
  },
  {
    path: 'crearOrdenDelDia/:id',
    component: CrearOrdenDelDiaComponent
  },
  {
    path: 'registrarSesionDeComiteTecnico/:id',
    component: RegistrarSesionComiteTecnicoComponent
  },
  {
    path: 'registrarSesionDeComiteTecnico/:id/registrarParticipantes',
    component: FormRegistrarParticipantesComponent
  },
  {
    path: 'crearActa/:id',
    component: CrearActaComponent
  },
  {
    path: 'crearActa/:id/observacion/:idsesionComiteSolicitud/:idcomiteTecnico/:idcontratacionProyecto/:idcontratacion',
    component: ObservacionComponent
  },
  {
    path: 'verificarCumplimiento/:id',
    component: TablaVerificarCumplimientoComponent
  },
  {
    path: 'verDetalleComiteTecnico/:id',
    component: RegistrarSesionComiteTecnicoComponent
  },
  {
    path: 'verDetalleEditarComiteTecnico/:id',
    component: RegistrarSesionComiteTecnicoComponent
  },
  {
    path: 'verDetalleEditarComiteTecnico/:id/verDetalleEditarParticipantes',
    component: FormRegistrarParticipantesComponent
  },
  {
    path: 'verDetalleComiteTecnico/:id/verDetalleParticipantes',
    component: FormRegistrarParticipantesComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ComiteTecnicoRoutingModule { }
