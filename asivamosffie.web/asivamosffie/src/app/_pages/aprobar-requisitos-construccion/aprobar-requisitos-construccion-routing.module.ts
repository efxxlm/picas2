import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'src/app/material/material.module';
import { QuillModule } from 'ngx-quill';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { AprobarRequisitosConstruccionComponent } from './components/aprobar-requisitos-construccion/aprobar-requisitos-construccion.component';



const routes: Routes = [
  {
    path: '',
    component: AprobarRequisitosConstruccionComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AprobarRequisitosConstruccionRoutingModule { }
