import { VerDetalleAvanceSemanalComponent } from './components/ver-detalle-avance-semanal/ver-detalle-avance-semanal.component';
import { ConsultarBitacoraComponent } from './components/consultar-bitacora/consultar-bitacora.component';
import { FormVerificarSeguimientoSemanalComponent } from './components/form-verificar-seguimiento-semanal/form-verificar-seguimiento-semanal.component';
import { VerificarAvanceSemanalComponent } from './components/verificar-avance-semanal/verificar-avance-semanal.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { VerDetalleMuestrasComponent } from './components/ver-detalle-muestras/ver-detalle-muestras.component';

const routes: Routes = [
  {
    path: '',
    component: VerificarAvanceSemanalComponent
  },
  {
    path: 'verificarSeguimientoSemanal/:id',
    component: FormVerificarSeguimientoSemanalComponent
  },
  {
    path: 'verificarSeguimientoSemanal/:id/verDetalleMuestras/:idEnsayo',
    component: VerDetalleMuestrasComponent
  },
  {
    path: 'consultarBitacora/:id',
    component: ConsultarBitacoraComponent
  },
  {
    path: 'consultarBitacora/:id/verDetalleAvanceSemanal/:idAvance',
    component: VerDetalleAvanceSemanalComponent
  },
  {
    path: 'consultarBitacora/:id/verDetalleAvanceSemanal/:idAvance/verDetalleMuestras/:idEnsayo',
    component: VerDetalleMuestrasComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VerificarAvanceSemanalRoutingModule { }
