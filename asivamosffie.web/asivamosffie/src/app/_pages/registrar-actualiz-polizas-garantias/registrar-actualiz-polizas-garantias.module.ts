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
import { RazonTipoActualizacionRapgComponent } from './components/razon-tipo-actualizacion-rapg/razon-tipo-actualizacion-rapg.component';
import { VigenciasValorRapgComponent } from './components/vigencias-valor-rapg/vigencias-valor-rapg.component';
import { ObservacionesEspecificasRapgComponent } from './components/observaciones-especificas-rapg/observaciones-especificas-rapg.component';
import { ListaChequeoRapgComponent } from './components/lista-chequeo-rapg/lista-chequeo-rapg.component';
import { RevisionAprobacionRapgComponent } from './components/revision-aprobacion-rapg/revision-aprobacion-rapg.component';
import { TablaObservacionesRevAprobRapgComponent } from './components/tabla-observaciones-rev-aprob-rapg/tabla-observaciones-rev-aprob-rapg.component';
import { VerDetalleEditarPolizaRapgComponent } from './components/ver-detalle-editar-poliza-rapg/ver-detalle-editar-poliza-rapg.component';
import { VerDetallePolizaRapgComponent } from './components/ver-detalle-poliza-rapg/ver-detalle-poliza-rapg.component';
import { DetalleTablaObservacionesRapgComponent } from './components/detalle-tabla-observaciones-rapg/detalle-tabla-observaciones-rapg.component';
import { AccordVigenciasValorComponent } from './components/accord-vigencias-valor/accord-vigencias-valor.component';

@NgModule({
  declarations: [RegistrarActualizPolizasGarantiasComponent, TablaGeneralRapgComponent, ActualizarPolizaRapgComponent, RazonTipoActualizacionRapgComponent, VigenciasValorRapgComponent, ObservacionesEspecificasRapgComponent, ListaChequeoRapgComponent, RevisionAprobacionRapgComponent, TablaObservacionesRevAprobRapgComponent, VerDetalleEditarPolizaRapgComponent, VerDetallePolizaRapgComponent, DetalleTablaObservacionesRapgComponent, AccordVigenciasValorComponent],
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
