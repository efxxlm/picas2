import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrarLiquidacionContratoComponent } from './components/registrar-liquidacion-contrato/registrar-liquidacion-contrato.component';
import { RegistrarLiquidacionContratoRoutingModule } from './registrar-liquidacion-contrato-routing.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { TablaSinRegistroLiquidacionRlcComponent } from './components/tabla-sin-registro-liquidacion-rlc/tabla-sin-registro-liquidacion-rlc.component';
import { GestionarSolicitudRlcComponent } from './components/gestionar-solicitud-rlc/gestionar-solicitud-rlc.component';
import { ActualizacionPolizaRlcComponent } from './components/actualizacion-poliza-rlc/actualizacion-poliza-rlc.component';
import { TablaBalanceFinancieroRlcComponent } from './components/tabla-balance-financiero-rlc/tabla-balance-financiero-rlc.component';
import { DetalleBalanceFinancieroRlcComponent } from './components/detalle-balance-financiero-rlc/detalle-balance-financiero-rlc.component';
import { RecursosComproPagadosRlcComponent } from './components/recursos-compro-pagados-rlc/recursos-compro-pagados-rlc.component';

@NgModule({
  declarations: [RegistrarLiquidacionContratoComponent, TablaSinRegistroLiquidacionRlcComponent, GestionarSolicitudRlcComponent, ActualizacionPolizaRlcComponent, TablaBalanceFinancieroRlcComponent, DetalleBalanceFinancieroRlcComponent, RecursosComproPagadosRlcComponent],
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
