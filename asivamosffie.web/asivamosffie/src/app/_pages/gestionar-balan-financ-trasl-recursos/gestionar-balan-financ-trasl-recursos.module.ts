import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GestionarBalanFinancTraslRecComponent } from './components/gestionar-balan-financ-trasl-rec/gestionar-balan-financ-trasl-rec.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { GestionarBalanFinancTraslRecursosRoutingModule } from './gestionar-balan-financ-trasl-recursos-routing.module';
import { ValidarBalanceGbftrecComponent } from './components/validar-balance-gbftrec/validar-balance-gbftrec.component';
import { RecursosCompromPagadosGbftrecComponent } from './components/recursos-comprom-pagados-gbftrec/recursos-comprom-pagados-gbftrec.component';
import { EjecucionFinancieraGbftrecComponent } from './components/ejecucion-financiera-gbftrec/ejecucion-financiera-gbftrec.component';
import { TrasladoRecursosGbftrecComponent } from './components/traslado-recursos-gbftrec/traslado-recursos-gbftrec.component';
import { RecursosCompromAccordGbftrecComponent } from './components/recursos-comprom-accord-gbftrec/recursos-comprom-accord-gbftrec.component';
import { FuentesUsosGbftrecComponent } from './components/fuentes-usos-gbftrec/fuentes-usos-gbftrec.component';

@NgModule({
  declarations: [GestionarBalanFinancTraslRecComponent, ValidarBalanceGbftrecComponent, RecursosCompromPagadosGbftrecComponent, EjecucionFinancieraGbftrecComponent, TrasladoRecursosGbftrecComponent, RecursosCompromAccordGbftrecComponent, FuentesUsosGbftrecComponent],
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
