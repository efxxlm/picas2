import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrarActualizPolizasGarantiasComponent } from './components/registrar-actualiz-polizas-garantias/registrar-actualiz-polizas-garantias.component';
import { RegistrarActualizPolizasGarantiasRoutingModule } from './registrar-actualiz-polizas-garantias-routing.module';
import { MaterialModule } from 'src/app/material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { QuillModule } from 'ngx-quill';
import { TablaGeneralRapgComponent } from './components/tabla-general-rapg/tabla-general-rapg.component';
import { ActualizarPolizaRapgComponent } from './components/actualizar-poliza-rapg/actualizar-poliza-rapg.component';

@NgModule({
  declarations: [RegistrarActualizPolizasGarantiasComponent, TablaGeneralRapgComponent, ActualizarPolizaRapgComponent],
  imports: [
    CommonModule,
    RegistrarActualizPolizasGarantiasRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot(),
    CurrencyMaskModule,
  ]
})
export class RegistrarActualizPolizasGarantiasModule { }
