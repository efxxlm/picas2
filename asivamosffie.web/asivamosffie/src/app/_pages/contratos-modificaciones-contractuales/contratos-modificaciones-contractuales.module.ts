import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ContratosModificacionesContractualesRoutingModule } from './contratos-modificaciones-contractuales-routing.module';
import { ContratosModificacionesContractualesComponent } from './components/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.component';
import { MaterialModule } from '../../material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TablaSinRegistroContratoComponent } from './components/tabla-sin-registro-contrato/tabla-sin-registro-contrato.component';
import { FormContratacionComponent } from './components/form-contratacion/form-contratacion.component';
import { FormModificacionContractualComponent } from './components/form-modificacion-contractual/form-modificacion-contractual.component';
import { FormRegistroTramiteComponent } from './components/form-registro-tramite/form-registro-tramite.component';
import { QuillModule } from 'ngx-quill';
import { TablaProcesoFirmasComponent } from './components/tabla-proceso-firmas/tabla-proceso-firmas.component';
import { FormRegistroModificacionContractualComponent } from './components/form-registro-modificacion-contractual/form-registro-modificacion-contractual.component';
import { TablaRegistradosComponent } from './components/tabla-registrados/tabla-registrados.component';


@NgModule({
  declarations: [ContratosModificacionesContractualesComponent, TablaSinRegistroContratoComponent, FormContratacionComponent, FormModificacionContractualComponent, FormRegistroTramiteComponent, TablaProcesoFirmasComponent, FormRegistroModificacionContractualComponent, TablaRegistradosComponent],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    QuillModule.forRoot(),
    ContratosModificacionesContractualesRoutingModule
  ]
})
export class ContratosModificacionesContractualesModule { }
