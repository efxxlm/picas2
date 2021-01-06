import { FormVerificarSeguimientoSemanalComponent } from './components/form-verificar-seguimiento-semanal/form-verificar-seguimiento-semanal.component';
import { VerificarAvanceSemanalComponent } from './components/verificar-avance-semanal/verificar-avance-semanal.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    component: VerificarAvanceSemanalComponent
  },
  {
    path: 'verificarSeguimientoSemanal/:id',
    component: FormVerificarSeguimientoSemanalComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VerificarAvanceSemanalRoutingModule { }
