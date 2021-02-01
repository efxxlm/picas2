import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrarPagosRendimientosComponent } from './components/registrar-pagos-rendimientos/registrar-pagos-rendimientos.component';
import { RegistrarPagosRendimientosRoutingModule } from './registrar-pagos-rendimientos-routing.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';

@NgModule({
  declarations: [RegistrarPagosRendimientosComponent],
  imports: [
    RegistrarPagosRendimientosRoutingModule,
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class RegistrarPagosRendimientosModule { }
