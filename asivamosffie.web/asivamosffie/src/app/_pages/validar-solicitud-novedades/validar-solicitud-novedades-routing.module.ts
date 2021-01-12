import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { ValidarNovedadContratoObraComponent } from './components/validar-novedad-contrato-obra/validar-novedad-contrato-obra.component';
import { ValidarNovedadContratoInterventoriaComponent } from './components/validar-novedad-contrato-interventoria/validar-novedad-contrato-interventoria.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'validarSolicitud',
    component: HomeComponent
  },
  {
    path: 'validarSolicitudNovedadObra/:id',
    component: ValidarNovedadContratoObraComponent
  },
  {
    path: 'verDetalleSolicitudNovedadObra/:id',
    component: ValidarNovedadContratoObraComponent
  },
  {
    path: 'validarSolicitudNovedadInterventoria/:id',
    component: ValidarNovedadContratoInterventoriaComponent
  },
  {
    path: 'verDetalleSolicitudNovedadInterventoria/:id',
    component: ValidarNovedadContratoInterventoriaComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ValidarSolicitudNovedadesRoutingModule { }
