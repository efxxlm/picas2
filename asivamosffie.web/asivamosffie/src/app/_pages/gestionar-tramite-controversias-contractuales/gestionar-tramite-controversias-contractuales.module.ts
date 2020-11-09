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
import { ControlYTablaReclamacionCcComponent } from './components/control-y-tabla-reclamacion-cc/control-y-tabla-reclamacion-cc.component';
import { RegistarReclamacionAseguradoraCcComponent } from './components/registar-reclamacion-aseguradora-cc/registar-reclamacion-aseguradora-cc.component';
import { VerdetalleReclamacionAsegCcComponent } from './components/verdetalle-reclamacion-aseg-cc/verdetalle-reclamacion-aseg-cc.component';
import { ActualizarReclamacionAsegCcComponent } from './components/actualizar-reclamacion-aseg-cc/actualizar-reclamacion-aseg-cc.component';
import { ControlYTablaActuacionReclamacionComponent } from './components/control-y-tabla-actuacion-reclamacion/control-y-tabla-actuacion-reclamacion.component';
import { RegistrarNuevaActuacionReclamacionComponent } from './components/registrar-nueva-actuacion-reclamacion/registrar-nueva-actuacion-reclamacion.component';
import { FormActuacionReclamacionComponent } from './components/form-actuacion-reclamacion/form-actuacion-reclamacion.component';
import { VerDetalleeditarActuacionReclmComponent } from './components/ver-detalleeditar-actuacion-reclm/ver-detalleeditar-actuacion-reclm.component';
import { VerdetalleReclamacionActuacionCcComponent } from './components/verdetalle-reclamacion-actuacion-cc/verdetalle-reclamacion-actuacion-cc.component';
import { ControlYTablaActuacionesNoTaiComponent } from './components/control-y-tabla-actuaciones-no-tai/control-y-tabla-actuaciones-no-tai.component';
import { RegistrarNuevaActuacionTramNoTaiComponent } from './components/registrar-nueva-actuacion-tram-no-tai/registrar-nueva-actuacion-tram-no-tai.component';
import { FormRegistarActuacionNotaiComponent } from './components/form-registar-actuacion-notai/form-registar-actuacion-notai.component';
import { VerDetalleeditarActuacionNotaiComponent } from './components/ver-detalleeditar-actuacion-notai/ver-detalleeditar-actuacion-notai.component';
import { VerDetalleActuacionNotaiComponent } from './components/ver-detalle-actuacion-notai/ver-detalle-actuacion-notai.component';
import { ControlYTablaMesasTrabajoCcComponent } from './components/control-y-tabla-mesas-trabajo-cc/control-y-tabla-mesas-trabajo-cc.component';



@NgModule({
  declarations: [ControlYTablaControversiasContractualesComponent, FormRegistrarControversiaContractuaComponent, GestionarTramiteControvrContractComponent, FormRegistrarControvrsAccordComponent, FormRegistrarControvrsSopSolComponent, ActualizarTramiteContvrContrcComponent, VerDetalleditarCntrvContrcComponent, ControlYTablaActuaTramiteCcComponent, RegistrarNuevaActuacionTramiteComponent, FormDescripcionActuacionComponent, FormReclamacionAseguradoraActuacionComponent, FormSoporteActuacionActtramComponent, VerdetalleeditTramiteCntrvContrcComponent, VerDetalleActuacionContrContrctComponent, DetalleDescrActCcComponent, DetalleReclmAseguradoraCcComponent, DetalleSopActuacionCcComponent, VerdetalleTramiteCcComponent, ControlYTablaReclamacionCcComponent, RegistarReclamacionAseguradoraCcComponent, VerdetalleReclamacionAsegCcComponent, ActualizarReclamacionAsegCcComponent, ControlYTablaActuacionReclamacionComponent, RegistrarNuevaActuacionReclamacionComponent, FormActuacionReclamacionComponent, VerDetalleeditarActuacionReclmComponent, VerdetalleReclamacionActuacionCcComponent, ControlYTablaActuacionesNoTaiComponent, RegistrarNuevaActuacionTramNoTaiComponent, FormRegistarActuacionNotaiComponent, VerDetalleeditarActuacionNotaiComponent, VerDetalleActuacionNotaiComponent, ControlYTablaMesasTrabajoCcComponent],
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
