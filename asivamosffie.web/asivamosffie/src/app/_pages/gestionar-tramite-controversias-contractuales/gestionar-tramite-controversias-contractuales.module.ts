import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QuillModule } from 'ngx-quill';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from 'src/app/material/material.module';
import { GestionarTramiteControversiasContractualesRoutingModule } from './gestionar-tramite-controversias-contractuales-routing.module';
import { ControlYTablaControversiasContractualesComponent } from './components/control-y-tabla-controversias-contractuales/control-y-tabla-controversias-contractuales.component';
import { FormRegistrarControversiaContractuaComponent } from './components/form-registrar-controversia-contractua/form-registrar-controversia-contractua.component';
import { GestionarTramiteControvrContractComponent } from './components/gestionar-tramite-controvr-contract/gestionar-tramite-controvr-contract.component';
import { FormRegistrarControvrsAccordComponent } from './components/form-registrar-controvrs-accord/form-registrar-controvrs-accord.component';
import { FormRegistrarControvrsSopSolComponent } from './components/form-registrar-controvrs-sop-sol/form-registrar-controvrs-sop-sol.component';
import { ActualizarTramiteContvrContrcComponent } from './components/actualizar-tramite-contvr-contrc/actualizar-tramite-contvr-contrc.component';
import { VerDetalleditarCntrvContrcComponent } from './components/ver-detalleditar-cntrv-contrc/ver-detalleditar-cntrv-contrc.component';
import { ControlYTablaActuaTramiteCcComponent } from './components/control-y-tabla-actua-tramite-cc/control-y-tabla-actua-tramite-cc.component';
import { RegistrarNuevaActuacionTramiteComponent } from './components/registrar-nueva-actuacion-tramite/registrar-nueva-actuacion-tramite.component';



@NgModule({
  declarations: [ControlYTablaControversiasContractualesComponent, FormRegistrarControversiaContractuaComponent, GestionarTramiteControvrContractComponent, FormRegistrarControvrsAccordComponent, FormRegistrarControvrsSopSolComponent, ActualizarTramiteContvrContrcComponent, VerDetalleditarCntrvContrcComponent, ControlYTablaActuaTramiteCcComponent, RegistrarNuevaActuacionTramiteComponent],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot(),
    GestionarTramiteControversiasContractualesRoutingModule
  ]
})
export class GestionarTramiteControversiasContractualesModule { }
