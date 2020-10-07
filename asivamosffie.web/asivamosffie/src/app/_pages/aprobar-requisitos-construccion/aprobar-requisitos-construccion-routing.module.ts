import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'src/app/material/material.module';
import { QuillModule } from 'ngx-quill';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { AprobarRequisitosConstruccionComponent } from './components/aprobar-requisitos-construccion/aprobar-requisitos-construccion.component';
import { FormValidacionRequisitosObraArtcComponent } from './components/form-validacion-requisitos-obra-artc/form-validacion-requisitos-obra-artc.component';
import { VerDetalleContratoObraArtcComponent } from './components/ver-detalle-contrato-obra-artc/ver-detalle-contrato-obra-artc.component';
import { FormValidacionRequisitosInterventoriaArtcComponent } from './components/form-validacion-requisitos-interventoria-artc/form-validacion-requisitos-interventoria-artc.component';
import { VerDetalleContratoInterventoriaArtcComponent } from './components/ver-detalle-contrato-interventoria-artc/ver-detalle-contrato-interventoria-artc.component';



const routes: Routes = [
  {
    path: '',
    component: AprobarRequisitosConstruccionComponent
  },
  {
    path: 'validarRequisitosInicioObra/:id',
    component: FormValidacionRequisitosObraArtcComponent
  },
  {
    path: 'verDetalleObra/:id',
    component: VerDetalleContratoObraArtcComponent
  },
  {
    path: 'verificarRequisitosInicioInterventoria/:id',
    component: FormValidacionRequisitosInterventoriaArtcComponent
  },
  {
    path: 'verDetalleInterventoria/:id',
    component: VerDetalleContratoInterventoriaArtcComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AprobarRequisitosConstruccionRoutingModule { }
