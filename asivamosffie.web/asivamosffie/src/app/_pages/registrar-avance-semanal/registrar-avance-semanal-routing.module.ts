import { VerDetalleMuestrasComponent } from './components/ver-detalle-muestras/ver-detalle-muestras.component';
import { VerDetalleAvanceSemanalComponent } from './components/ver-detalle-avance-semanal/ver-detalle-avance-semanal.component';
import { ConsultarEditarBitacoraComponent } from './components/consultar-editar-bitacora/consultar-editar-bitacora.component';
import { RegistrarResultadosEnsayoComponent } from './components/registrar-resultados-ensayo/registrar-resultados-ensayo.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FormRegistrarSeguimientoSemanalComponent } from './components/form-registrar-seguimiento-semanal/form-registrar-seguimiento-semanal.component';
import { RegistrarAvanceSemanalComponent } from './components/registrar-avance-semanal/registrar-avance-semanal.component';

const routes: Routes = [
  {
    path: '',
    component: RegistrarAvanceSemanalComponent
  },
  {
    path: 'registroSeguimientoSemanal/:id',
    component: FormRegistrarSeguimientoSemanalComponent
  },
  {
    path: 'registroSeguimientoSemanal/:id/registroResultadosEnsayo/:idEnsayo',
    component: RegistrarResultadosEnsayoComponent
  },
  {
    path: 'consultarEditarBitacora/:id',
    component: ConsultarEditarBitacoraComponent
  },
  {
    path: 'consultarEditarBitacora/:id/verDetalleAvanceSemanal/:idAvance',
    component: VerDetalleAvanceSemanalComponent
  },
  {
    path: 'consultarEditarBitacora/:id/verDetalleAvanceSemanal/:idAvance/registroResultadosEnsayo/:idEnsayo',
    component: RegistrarResultadosEnsayoComponent
  },
  {
    path: 'consultarEditarBitacora/:id/verDetalleAvanceSemanal/:idAvance/verDetalleMuestras/:idEnsayo',
    component: VerDetalleMuestrasComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistrarAvanceSemanalRoutingModule { }
