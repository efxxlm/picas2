import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActualizarTramiteRaccComponent } from './components/actualizar-tramite-racc/actualizar-tramite-racc.component';
import { RegistrarActuacionControvContrcComponent } from './components/registrar-actuacion-controv-contrc/registrar-actuacion-controv-contrc.component';

const routes: Routes = [
  {
    path: '',
    component: RegistrarActuacionControvContrcComponent
  },
  {
    path: 'actualizarTramite/:id',
    component: ActualizarTramiteRaccComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistrarActuacionControvContrcRoutingModule { }
