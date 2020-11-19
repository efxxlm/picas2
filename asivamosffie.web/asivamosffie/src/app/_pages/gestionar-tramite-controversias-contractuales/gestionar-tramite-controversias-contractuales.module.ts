import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QuillModule } from 'ngx-quill';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from 'src/app/material/material.module';
import { GestionarTramiteControversiasContractualesRoutingModule } from './gestionar-tramite-controversias-contractuales-routing.module';
import { FormRegistrarControversiaContractuaComponent } from './components/form-registrar-controversia-contractua/form-registrar-controversia-contractua.component';
import { GestionarTramiteControvrContractComponent } from './components/gestionar-tramite-controvr-contract/gestionar-tramite-controvr-contract.component';
import { FormRegistrarControvrsAccordComponent } from './components/form-registrar-controvrs-accord/form-registrar-controvrs-accord.component';
import { FormRegistrarControvrsSopSolComponent } from './components/form-registrar-controvrs-sop-sol/form-registrar-controvrs-sop-sol.component';
import { ActualizarTramiteContvrContrcComponent } from './components/actualizar-tramite-contvr-contrc/actualizar-tramite-contvr-contrc.component';
import { VerDetalleditarCntrvContrcComponent } from './components/ver-detalleditar-cntrv-contrc/ver-detalleditar-cntrv-contrc.component';
import { ControlYTablaActuaTramiteCcComponent } from './components/control-y-tabla-actua-tramite-cc/control-y-tabla-actua-tramite-cc.component';
import { RegistrarNuevaActuacionTramiteComponent } from './components/registrar-nueva-actuacion-tramite/registrar-nueva-actuacion-tramite.component';
import { FormDescripcionActuacionComponent } from './components/form-descripcion-actuacion/form-descripcion-actuacion.component';
import { FormReclamacionAseguradoraActuacionComponent } from './components/form-reclamacion-aseguradora-actuacion/form-reclamacion-aseguradora-actuacion.component';
import { FormSoporteActuacionActtramComponent } from './components/form-soporte-actuacion-acttram/form-soporte-actuacion-acttram.component';
import { VerdetalleeditTramiteCntrvContrcComponent } from './components/verdetalleedit-tramite-cntrv-contrc/verdetalleedit-tramite-cntrv-contrc.component';
import { VerDetalleActuacionContrContrctComponent } from './components/ver-detalle-actuacion-contr-contrct/ver-detalle-actuacion-contr-contrct.component';
import { DetalleDescrActCcComponent } from './components/detalle-descr-act-cc/detalle-descr-act-cc.component';
import { DetalleReclmAseguradoraCcComponent } from './components/detalle-reclm-aseguradora-cc/detalle-reclm-aseguradora-cc.component';
import { DetalleSopActuacionCcComponent } from './components/detalle-sop-actuacion-cc/detalle-sop-actuacion-cc.component';
import { VerdetalleTramiteCcComponent } from './components/verdetalle-tramite-cc/verdetalle-tramite-cc.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';



@NgModule({
  declarations: [FormRegistrarControversiaContractuaComponent, GestionarTramiteControvrContractComponent, FormRegistrarControvrsAccordComponent, FormRegistrarControvrsSopSolComponent, ActualizarTramiteContvrContrcComponent, VerDetalleditarCntrvContrcComponent, ControlYTablaActuaTramiteCcComponent, RegistrarNuevaActuacionTramiteComponent, FormDescripcionActuacionComponent, FormReclamacionAseguradoraActuacionComponent, FormSoporteActuacionActtramComponent, VerdetalleeditTramiteCntrvContrcComponent, VerDetalleActuacionContrContrctComponent, DetalleDescrActCcComponent, DetalleReclmAseguradoraCcComponent, DetalleSopActuacionCcComponent, VerdetalleTramiteCcComponent],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot(),
    GestionarTramiteControversiasContractualesRoutingModule
  ]
})
export class GestionarTramiteControversiasContractualesModule { }
