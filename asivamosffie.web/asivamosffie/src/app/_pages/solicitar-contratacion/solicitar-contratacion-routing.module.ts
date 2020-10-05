import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SolicitarContratacionComponent } from './components/solicitar-contratacion/solicitar-contratacion.component';
import { FormSolicitarContratacionComponent } from './components/form-solicitar-contratacion/form-solicitar-contratacion.component';
import { ExpansionPanelDetallarSolicitudComponent } from './components/expansion-panel-detallar-solicitud/expansion-panel-detallar-solicitud.component';
import { DefinirCaracteristicasComponent } from './components/definir-caracteristicas/definir-caracteristicas.component';
import { DefinirFuentesYUsosComponent } from './components/definir-fuentes-y-usos/definir-fuentes-y-usos.component';
import { VerDetalleContratacionComponent } from './components/ver-detalle-contratacion/ver-detalle-contratacion.component';

const routes: Routes = [
  {
    path: '',
    component: SolicitarContratacionComponent
  },
  {
    path: 'solicitar',
    component: FormSolicitarContratacionComponent
  },
  {
    path: 'solicitud/:id',
    component: ExpansionPanelDetallarSolicitudComponent
  },
  {
    path: 'definir-caracteristicas/:id',
    component: DefinirCaracteristicasComponent
  },
  {
    path: 'definir-fuentes/:id',
    component: DefinirFuentesYUsosComponent
  },
  {
    path: 'verDetalleContratacion/:id',
    component: VerDetalleContratacionComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SolicitarContratacionRoutingModule { }
