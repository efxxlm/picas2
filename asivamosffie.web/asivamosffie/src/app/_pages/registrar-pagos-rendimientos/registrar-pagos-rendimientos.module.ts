import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrarPagosRendimientosComponent } from './components/registrar-pagos-rendimientos/registrar-pagos-rendimientos.component';
import { RegistrarPagosRendimientosRoutingModule } from './registrar-pagos-rendimientos-routing.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { TablaRegistrarPagosRprComponent } from './components/tabla-registrar-pagos-rpr/tabla-registrar-pagos-rpr.component';
import { TablaRegistrarRendimientosRprComponent } from './components/tabla-registrar-rendimientos-rpr/tabla-registrar-rendimientos-rpr.component';
import { DialogCargarReportPagosRprComponent } from './components/dialog-cargar-report-pagos-rpr/dialog-cargar-report-pagos-rpr.component';
import { DialogCargarReportRendRprComponent } from './components/dialog-cargar-report-rend-rpr/dialog-cargar-report-rend-rpr.component';
import { ObservacionesReportPagoRprComponent } from './components/observaciones-report-pago-rpr/observaciones-report-pago-rpr.component';
import { ObservacionesReportRendRprComponent } from './components/observaciones-report-rend-rpr/observaciones-report-rend-rpr.component';

@NgModule({
  declarations: [RegistrarPagosRendimientosComponent, TablaRegistrarPagosRprComponent, TablaRegistrarRendimientosRprComponent, DialogCargarReportPagosRprComponent, DialogCargarReportRendRprComponent, ObservacionesReportPagoRprComponent, ObservacionesReportRendRprComponent],
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
