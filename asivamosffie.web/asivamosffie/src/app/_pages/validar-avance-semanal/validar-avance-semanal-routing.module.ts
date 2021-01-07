import { VerDetalleMuestrasComponent } from './components/ver-detalle-muestras/ver-detalle-muestras.component';
import { ConsultarBitacoraComponent } from './components/consultar-bitacora/consultar-bitacora.component';
import { FormValidarSeguimientoSemanalComponent } from './components/form-validar-seguimiento-semanal/form-validar-seguimiento-semanal.component';
import { ValidarAvanceSemanalComponent } from './components/validar-avance-semanal/validar-avance-semanal.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { VerDetalleAvanceSemanalComponent } from './components/ver-detalle-avance-semanal/ver-detalle-avance-semanal.component';


const routes: Routes = [
  {
    path: '',
    component: ValidarAvanceSemanalComponent
  },
  {
    path: 'validarSeguimientoSemanal/:id',
    component: FormValidarSeguimientoSemanalComponent
  },
  {
    path: 'validarSeguimientoSemanal/:id/verDetalleMuestras/:idEnsayo',
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
export class ValidarAvanceSemanalRoutingModule { }
