import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ComiteFiduciarioComponent } from './components/comite-fiduciario/comite-fiduciario.component';
import { CrearOrdenDelDiaComponent } from './components/crear-orden-del-dia/crear-orden-del-dia.component';
import { FormRegistrarParticipantesComponent } from './components/form-registrar-participantes/form-registrar-participantes.component';
import { CrearActaComponent } from './components/crear-acta/crear-acta.component';
import { RegistrarSesionComiteFiduciarioComponent } from './components/registrar-sesion-comite-fiduciario/registrar-sesion-comite-fiduciario.component';
import { TablaVerificarCumplimientosComponent } from './components/tabla-verificar-cumplimientos/tabla-verificar-cumplimientos.component';

const routes: Routes = [
  {
    path: '',
    component: ComiteFiduciarioComponent
  },
  {
    path: 'crearOrdenDelDia/:id',
    component: CrearOrdenDelDiaComponent
  },
  {
    path: 'registrarSesionDeComiteFiduciario/:id',
    component: RegistrarSesionComiteFiduciarioComponent
  },
  {
    path: 'registrarSesionDeComiteFiduciario/:id/registrarParticipantes',
    component: FormRegistrarParticipantesComponent
  },
  {
    path: 'crearActa/:id',
    component: CrearActaComponent
  },
  {
    path: 'verificarCumplimiento/:id',
    component: TablaVerificarCumplimientosComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ComiteFiduciarioRoutingModule { }
