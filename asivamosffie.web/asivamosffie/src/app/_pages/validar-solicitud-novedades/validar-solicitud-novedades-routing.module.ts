import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { ValidarNovedadContratoObraComponent } from './components/validar-novedad-contrato-obra/validar-novedad-contrato-obra.component';
import { ValidarNovedadContratoInterventoriaComponent } from './components/validar-novedad-contrato-interventoria/validar-novedad-contrato-interventoria.component';
import { VerDetalleSolicNovIntervnComponent } from './components/ver-detalle-solic-nov-intervn/ver-detalle-solic-nov-intervn.component';
import { VerDetalleSolicNovObraComponent } from './components/ver-detalle-solic-nov-obra/ver-detalle-solic-nov-obra.component';

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
    path: 'verDetalleEditarSolicitudNovedadObra/:id',
    component: ValidarNovedadContratoObraComponent
  },
  {
    path: 'verDetalleSolicitudNovedadObra/:id',
    component: VerDetalleSolicNovObraComponent
  },
  {
    path: 'validarSolicitudNovedadInterventoria/:id',
    component: ValidarNovedadContratoInterventoriaComponent
  },
  {
    path: 'verDetalleEditarSolicitudNovedadInterventoria/:id',
    component: ValidarNovedadContratoInterventoriaComponent
  },
  {
    path: 'verDetalleSolicitudNovedadInterventoria/:id',
    component: VerDetalleSolicNovIntervnComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ValidarSolicitudNovedadesRoutingModule { }
