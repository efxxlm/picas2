import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { VerificarRequisitosConstruccionComponent } from './components/verificar-requisitos-construccion/verificar-requisitos-construccion.component';
import { FormVerificacionRequisitosComponent } from './components/form-verificacion-requisitos/form-verificacion-requisitos.component';
import { VerdetalleObraVrtcComponent } from './components/verdetalle-obra-vrtc/verdetalle-obra-vrtc.component';
import { VerdetalleInterventoriaVrtcComponent } from './components/verdetalle-interventoria-vrtc/verdetalle-interventoria-vrtc.component';
import { FormInterventoriaVerificacionRequisitosComponent } from './components/form-interventoria-verificacion-requisitos/form-interventoria-verificacion-requisitos.component';

const routes: Routes = [
  {
    path: '',
    component: VerificarRequisitosConstruccionComponent
  },
  {
    path:'verificarRequisitosInicio/:id',
    component: FormVerificacionRequisitosComponent
  },
  {
    path: 'verificarRequisitosInicioInterventoria/:id',
    component: FormInterventoriaVerificacionRequisitosComponent
  },
  {
    path:'verDetalleObra/:id',
    component: VerdetalleObraVrtcComponent
  },
  {
    path:'verDetalleInterventoria/:id',
    component: VerdetalleInterventoriaVrtcComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VerificarRequisitosConstruccionRoutingModule { }