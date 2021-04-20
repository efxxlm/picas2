import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { VerBitacoraComponent } from './components/ver-bitacora/ver-bitacora.component';
import { VerificarSeguimientoComponent } from './components/verificar-seguimiento/verificar-seguimiento.component';
import { VerDetalleRegistroComponent } from './components/ver-detalle-registro/ver-detalle-registro.component'

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'verificarSeguimiento/:id',
    component: VerificarSeguimientoComponent
  },
  {
    path: 'verBitacora/:id',
    component: VerBitacoraComponent
  },
  {
    path: 'verBitacora/:id/verDetalle/:id',
    component: VerDetalleRegistroComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VerificarSeguimientoDiarioRoutingModule { }
