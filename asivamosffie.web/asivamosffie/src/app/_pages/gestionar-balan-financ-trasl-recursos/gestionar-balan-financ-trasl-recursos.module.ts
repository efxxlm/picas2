import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GestionarBalanFinancTraslRecComponent } from './components/gestionar-balan-financ-trasl-rec/gestionar-balan-financ-trasl-rec.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { GestionarBalanFinancTraslRecursosRoutingModule } from './gestionar-balan-financ-trasl-recursos-routing.module';
import { ValidarBalanceGbftrecComponent } from './components/validar-balance-gbftrec/validar-balance-gbftrec.component';

@NgModule({
  declarations: [GestionarBalanFinancTraslRecComponent, ValidarBalanceGbftrecComponent],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot(),
    GestionarBalanFinancTraslRecursosRoutingModule
  ]
})
export class GestionarBalanFinancTraslRecursosModule { }
