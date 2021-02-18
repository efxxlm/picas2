import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GestionarTramiteLiqContractualRoutingModule } from './gestionar-tramite-liq-contractual-routing.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { GestionarTramiteLiqContractualComponent } from './components/gestionar-tramite-liq-contractual/gestionar-tramite-liq-contractual.component';
import { TablaLiquidacionObraGtlcComponent } from './components/tabla-liquidacion-obra-gtlc/tabla-liquidacion-obra-gtlc.component';
import { TablaLiquidacionIntervnGtlcComponent } from './components/tabla-liquidacion-intervn-gtlc/tabla-liquidacion-intervn-gtlc.component';



@NgModule({
  declarations: [GestionarTramiteLiqContractualComponent, TablaLiquidacionObraGtlcComponent, TablaLiquidacionIntervnGtlcComponent],
  imports: [
    CommonModule,
    GestionarTramiteLiqContractualRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot(),
    CurrencyMaskModule,
  ]
})
export class GestionarTramiteLiqContractualModule { }
