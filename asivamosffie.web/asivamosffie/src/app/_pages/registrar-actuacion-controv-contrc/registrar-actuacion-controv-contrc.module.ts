import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrarActuacionControvContrcComponent } from './components/registrar-actuacion-controv-contrc/registrar-actuacion-controv-contrc.component';
import { MaterialModule } from 'src/app/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { RegistrarActuacionControvContrcRoutingModule } from './registrar-actuacion-controv-contrc-routing.module';



@NgModule({
  declarations: [RegistrarActuacionControvContrcComponent],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot(),
    RegistrarActuacionControvContrcRoutingModule
  ]
})
export class RegistrarActuacionControvContrcModule { }
