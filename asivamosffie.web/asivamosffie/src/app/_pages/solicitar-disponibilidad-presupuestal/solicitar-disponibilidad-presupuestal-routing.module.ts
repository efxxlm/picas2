import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TituloComponent } from './components/titulo/titulo.component';
import { TablaCrearSolicitudTradicionalComponent } from './components/tabla-crear-solicitud-tradicional/tabla-crear-solicitud-tradicional.component';
import { RegistrarInformacionAdicionalComponent } from './components/registrar-informacion-adicional/registrar-informacion-adicional.component';
import { CrearSolicitudEspecialComponent } from './components/crear-solicitud-especial/crear-solicitud-especial.component';
import { NuevaSolicitudEspecialComponent } from './components/nueva-solicitud-especial/nueva-solicitud-especial.component';
import { CrearDisponibilidadPresupuestalAdministrativoComponent } from './components/crear-disponibilidad-presupuestal-administrativo/crear-administrativo.component';
import { DetalleDisponibilidadPresupuestalComponent } from './components/detalle-disponibilidad-presupuestal/detalle-disponibilidad-presupuestal.component';
import { VerDetalleDdpEspecialComponent } from './components/ver-detalle-ddp-especial/ver-detalle-ddp-especial.component';

const routes: Routes = [
  {
    path: '',
    component: TituloComponent
  },
  {
    path: 'crearSolicitudTradicional',
    component: TablaCrearSolicitudTradicionalComponent
  },
  {
    path: 'crearSolicitudTradicional/registrar/:idContratacion/:idDisponibilidadPresupuestal/:idTipoSolicitud',
    component: RegistrarInformacionAdicionalComponent
  },
  {
    path: 'crearSolicitudTradicional/verDetalle/:idDisponibilidadPresupuestal/:idTipoSolicitud/:id',
    component: DetalleDisponibilidadPresupuestalComponent
  },
  {
    path: 'crearSolicitudEspecial',
    component: CrearSolicitudEspecialComponent
  },
  {
    path: 'crearSolicitudEspecial/nueva/:id',
    component: NuevaSolicitudEspecialComponent
  },
  {
    path: 'crearSolicitudAdministrativa/nueva/:id',
    component: CrearDisponibilidadPresupuestalAdministrativoComponent
  },
  {
    path: 'verDetalleDDPEspecial/:id',
    component: VerDetalleDdpEspecialComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SolicitarDisponibilidadPresupuestalRoutingModule { }
