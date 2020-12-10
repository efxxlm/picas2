import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GestionarProcesosDefensaJudicialComponent } from './components/gestionar-procesos-defensa-judicial/gestionar-procesos-defensa-judicial.component';
import { MaterialModule } from 'src/app/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { GestionarProcesosDefensaJudicialRoutingModule } from './gestionar-procesos-defensa-judicial-routing.module';
import { ControlTablaProcesoDefensaJudicialComponent } from './components/control-tabla-proceso-defensa-judicial/control-tabla-proceso-defensa-judicial.component';
import { RegistroNuevoProcesoJudicialComponent } from './components/registro-nuevo-proceso-judicial/registro-nuevo-proceso-judicial.component';
import { FormContratosAsociadosDjComponent } from './components/form-contratos-asociados-dj/form-contratos-asociados-dj.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { FormDetalleProcesoDjComponent } from './components/form-detalle-proceso-dj/form-detalle-proceso-dj.component';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { FormConvocadosDjComponent } from './components/form-convocados-dj/form-convocados-dj.component';
import { FormUrlsoporteDjComponent } from './components/form-urlsoporte-dj/form-urlsoporte-dj.component';
import { FormFichaEstudioDjComponent } from './components/form-ficha-estudio-dj/form-ficha-estudio-dj.component';



@NgModule({
  declarations: [GestionarProcesosDefensaJudicialComponent, ControlTablaProcesoDefensaJudicialComponent, RegistroNuevoProcesoJudicialComponent, FormContratosAsociadosDjComponent, FormDetalleProcesoDjComponent, FormConvocadosDjComponent, FormUrlsoporteDjComponent, FormFichaEstudioDjComponent],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    GestionarProcesosDefensaJudicialRoutingModule,
    QuillModule.forRoot(),
    CurrencyMaskModule
  ]
})
export class GestionarProcesosDefensaJudicialModule { }
