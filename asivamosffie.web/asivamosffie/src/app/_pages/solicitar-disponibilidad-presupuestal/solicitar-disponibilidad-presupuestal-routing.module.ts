import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TituloComponent } from './components/titulo/titulo.component';
import { TablaCrearSolicitudTradicionalComponent } from './components/tabla-crear-solicitud-tradicional/tabla-crear-solicitud-tradicional.component';
import { RegistrarInformacionAdicionalComponent } from './components/registrar-informacion-adicional/registrar-informacion-adicional.component';
import { CrearSolicitudEspecialComponent } from './components/crear-solicitud-especial/crear-solicitud-especial.component';
import { NuevaSolicitudEspecialComponent } from './components/nueva-solicitud-especial/nueva-solicitud-especial.component';

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
    path: 'crearSolicitudTradicional/registrar/:idContratacion/:idDisponibilidadPresupuestal',
    component: RegistrarInformacionAdicionalComponent
  },
  {
    path: 'crearSolicitudEspecial',
    component: CrearSolicitudEspecialComponent
  },
  {
    path: 'crearSolicitudEspecial/nueva',
    component: NuevaSolicitudEspecialComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SolicitarDisponibilidadPresupuestalRoutingModule { }
