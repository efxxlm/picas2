import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { VerificarSolicitudNovedadComponent } from './components/verificar-solicitud-novedad/verificar-solicitud-novedad.component';
import { VerDetalleSolicitudComponent } from './components/ver-detalle-solicitud/ver-detalle-solicitud.component';
import { RegistrarSolicitudComponent } from './components/registrar-solicitud/registrar-solicitud.component';
import { VerDetalleInterventoriaComponent } from './components/ver-detalle-interventoria/ver-detalle-interventoria.component';
import { RegistrarSolicitudInterventoriaComponent } from './components/registrar-solicitud-interventoria/registrar-solicitud-interventoria.component';


const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'verificarSolicitudNovedad/:id',
    component: VerificarSolicitudNovedadComponent
  },
  {
    path: 'verDetalleSolicitudNovedadObra/:id',
    component: VerDetalleSolicitudComponent
  },
  {
    path: 'registrarSolicitud',
    component: RegistrarSolicitudComponent
  },
  {
    path: 'verDetalleSolicitudNovedadInterventoria/:id',
    component: VerDetalleInterventoriaComponent
  },
  {
    path: 'registrarSolicitudInterventoria',
    component: RegistrarSolicitudInterventoriaComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VerificarSolicitudDeNovedadesRoutingModule { }
