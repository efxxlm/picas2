import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActualizarTramiteRaccComponent } from './components/actualizar-tramite-racc/actualizar-tramite-racc.component';
import { RegistrarActuacionControvContrcComponent } from './components/registrar-actuacion-controv-contrc/registrar-actuacion-controv-contrc.component';
import { RegistrarAvanceActuaDerivadasComponent } from './components/registrar-avance-actua-derivadas/registrar-avance-actua-derivadas.component';

const routes: Routes = [
  {
    path: '',
    component: RegistrarActuacionControvContrcComponent
  },
  {
    path: 'actualizarTramite/:id',
    component: ActualizarTramiteRaccComponent
  },
  {
    path:'registrarActuacionDerivada',
    component: RegistrarAvanceActuaDerivadasComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistrarActuacionControvContrcRoutingModule { }
