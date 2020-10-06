import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'src/app/material/material.module';
import { QuillModule } from 'ngx-quill';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { AprobarRequisitosConstruccionComponent } from './components/aprobar-requisitos-construccion/aprobar-requisitos-construccion.component';
import { FormValidacionRequisitosObraArtcComponent } from './components/form-validacion-requisitos-obra-artc/form-validacion-requisitos-obra-artc.component';



const routes: Routes = [
  {
    path: '',
    component: AprobarRequisitosConstruccionComponent
  },
  {
    path: 'validarRequisitosInicio/:id',
    component: FormValidacionRequisitosObraArtcComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AprobarRequisitosConstruccionRoutingModule { }
