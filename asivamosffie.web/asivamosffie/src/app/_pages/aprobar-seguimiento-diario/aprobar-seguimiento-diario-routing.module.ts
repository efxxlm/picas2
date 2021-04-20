import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { ValidarSeguimientoDiarioComponent } from './components/validar-seguimiento-diario/validar-seguimiento-diario.component';
import { VerBitacoraComponent } from './components/ver-bitacora/ver-bitacora.component'
import { VerDetalleRegistroComponent } from './components/ver-detalle-registro/ver-detalle-registro.component'

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'validarSeguimiento/:id',
    component: ValidarSeguimientoDiarioComponent
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
export class AprobarSeguimientoDiarioRoutingModule { }
