import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RequisitosTecnicosConstruccionComponent } from './components/requisitos-tecnicos-construccion/requisitos-tecnicos-construccion.component';
import { FormRequisitosTecnicosConstruccionComponent } from './components/form-requisitos-tecnicos-construccion/form-requisitos-tecnicos-construccion.component';
import { VerDetalleReqTecConstrComponent } from './components/ver-detalle-req-tec-constr/ver-detalle-req-tec-constr.component';

const routes: Routes = [
  {
    path: '',
    component: RequisitosTecnicosConstruccionComponent
  },
  {
    path: 'gestionarInicioContrato/:id',
    component: FormRequisitosTecnicosConstruccionComponent
  },
  {
    path: 'verDetalles/:id',
    component: VerDetalleReqTecConstrComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RequisitosTecnicosConstruccionRoutingModule { }
