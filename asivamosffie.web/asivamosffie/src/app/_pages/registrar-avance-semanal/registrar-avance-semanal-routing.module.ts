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
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistrarAvanceSemanalRoutingModule { }
