import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrarLiquidacionContratoComponent } from './components/registrar-liquidacion-contrato/registrar-liquidacion-contrato.component';
import { RegistrarLiquidacionContratoRoutingModule } from './registrar-liquidacion-contrato-routing.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';

@NgModule({
  declarations: [RegistrarLiquidacionContratoComponent],
  imports: [
    CommonModule,
    RegistrarLiquidacionContratoRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot(),
    CurrencyMaskModule,
  ]
})
export class RegistrarLiquidacionContratoModule { }
