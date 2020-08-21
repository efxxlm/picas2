import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SesionComiteFiduciarioComponent } from './components/sesion-comite-fiduciario/sesion-comite-fiduciario.component';
import { OrdenesDiaComponent } from './components/ordenes-dia/ordenes-dia.component';
import { CrearOrdenComponent } from './components/crear-orden/crear-orden.component';
import { SesionesConvocadasComponent } from './components/sesiones-convocadas/sesiones-convocadas.component';
import { EditarOrdenComponent } from './components/editar-orden/editar-orden.component';
import { RegistrarSesionComponent } from './components/registrar-sesion/registrar-sesion.component';


const routes: Routes = [
  {
    path: '',
    component: SesionComiteFiduciarioComponent,
    children: [
      {
        path: 'ordenesDelDia',
        component: OrdenesDiaComponent
      },
      {
        path: 'sesionesConvocadas',
        component: SesionesConvocadasComponent
      }
    ]
  },
  {
    path: 'crearOrden',
    component: CrearOrdenComponent
  },
  {
    path: 'editarOrden',
    component: EditarOrdenComponent
  },
  {
    path: 'registrarSesion',
    component: RegistrarSesionComponent
  },
  {
    path: '**',
    pathMatch: 'full',
    redirectTo: '/comiteFiduciario'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SesionComiteFiduciarioRoutingModule { }
