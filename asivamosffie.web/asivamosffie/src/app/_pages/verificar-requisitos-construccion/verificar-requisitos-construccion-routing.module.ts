import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { VerificarRequisitosConstruccionComponent } from './components/verificar-requisitos-construccion/verificar-requisitos-construccion.component';
import { FormVerificacionRequisitosComponent } from './components/form-verificacion-requisitos/form-verificacion-requisitos.component';

const routes: Routes = [
  {
    path: '',
    component: VerificarRequisitosConstruccionComponent
  },
  {
    path:'verificarRequisitosInicio/:id',
    component: FormVerificacionRequisitosComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VerificarRequisitosConstruccionRoutingModule { }