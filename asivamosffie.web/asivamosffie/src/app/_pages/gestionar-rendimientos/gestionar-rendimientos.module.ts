import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GestionarRendimientosRoutingModule } from './gestionar-rendimientos-routing.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { GestionarRendimientosComponent } from './components/gestionar-rendimientos/gestionar-rendimientos.component';

@NgModule({
  declarations: [GestionarRendimientosComponent],
  imports: [
    CommonModule,
    GestionarRendimientosRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class GestionarRendimientosModule { }
