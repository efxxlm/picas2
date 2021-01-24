import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { FormRegistrarSeguimientoComponent } from './components/form-registrar-seguimiento/form-registrar-seguimiento.component';
import { VerDetalleRegistroComponent } from './components/ver-detalle-registro/ver-detalle-registro.component';
import { VerBitacoraComponent } from './components/ver-bitacora/ver-bitacora.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'registrarSeguimiento/:id',
    component: FormRegistrarSeguimientoComponent
  },
  {
    path: 'verBitacora/:id',
    component: VerBitacoraComponent
  },
  {
    path: 'verDetalle/:id',
    component: VerDetalleRegistroComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistroSeguimientoDiarioRoutingModule { }
