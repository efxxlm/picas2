import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActualizarMesaDeTrabajoComponent } from './components/actualizar-mesa-de-trabajo/actualizar-mesa-de-trabajo.component';
import { ActualizarReclamacionAsegCcComponent } from './components/actualizar-reclamacion-aseg-cc/actualizar-reclamacion-aseg-cc.component';
import { ActualizarTramiteContvrContrcComponent } from './components/actualizar-tramite-contvr-contrc/actualizar-tramite-contvr-contrc.component';
import { FormRegistrarControversiaContractuaComponent } from './components/form-registrar-controversia-contractua/form-registrar-controversia-contractua.component';
import { GestionarTramiteControvrContractComponent } from './components/gestionar-tramite-controvr-contract/gestionar-tramite-controvr-contract.component';
import { RegistarReclamacionAseguradoraCcComponent } from './components/registar-reclamacion-aseguradora-cc/registar-reclamacion-aseguradora-cc.component';
import { RegistrarNuevaActuacionReclamacionComponent } from './components/registrar-nueva-actuacion-reclamacion/registrar-nueva-actuacion-reclamacion.component';
import { RegistrarNuevaActuacionTramNoTaiComponent } from './components/registrar-nueva-actuacion-tram-no-tai/registrar-nueva-actuacion-tram-no-tai.component';
import { RegistrarNuevaActuacionTramiteComponent } from './components/registrar-nueva-actuacion-tramite/registrar-nueva-actuacion-tramite.component';
import { RegistrarNuevaMesatrabajoActComponent } from './components/registrar-nueva-mesatrabajo-act/registrar-nueva-mesatrabajo-act.component';
import { RegistrarNuevaMesatrabajoCcComponent } from './components/registrar-nueva-mesatrabajo-cc/registrar-nueva-mesatrabajo-cc.component';
import { VerDetalleActuacionContrContrctComponent } from './components/ver-detalle-actuacion-contr-contrct/ver-detalle-actuacion-contr-contrct.component';
import { VerDetalleActuacionMtComponent } from './components/ver-detalle-actuacion-mt/ver-detalle-actuacion-mt.component';
import { VerDetalleActuacionNotaiComponent } from './components/ver-detalle-actuacion-notai/ver-detalle-actuacion-notai.component';
import { VerDetalleMesaTrabajoComponent } from './components/ver-detalle-mesa-trabajo/ver-detalle-mesa-trabajo.component';
import { VerDetalleditarCntrvContrcComponent } from './components/ver-detalleditar-cntrv-contrc/ver-detalleditar-cntrv-contrc.component';
import { VerDetalleeditarActuacionMtComponent } from './components/ver-detalleeditar-actuacion-mt/ver-detalleeditar-actuacion-mt.component';
import { VerDetalleeditarActuacionNotaiComponent } from './components/ver-detalleeditar-actuacion-notai/ver-detalleeditar-actuacion-notai.component';
import { VerDetalleeditarActuacionReclmComponent } from './components/ver-detalleeditar-actuacion-reclm/ver-detalleeditar-actuacion-reclm.component';
import { VerDetalleeditarMesaDeTrabajoComponent } from './components/ver-detalleeditar-mesa-de-trabajo/ver-detalleeditar-mesa-de-trabajo.component';
import { VerDetalleeditarReclamacionComponent } from './components/ver-detalleeditar-reclamacion/ver-detalleeditar-reclamacion.component';
import { VerdetalleReclamacionActuacionCcComponent } from './components/verdetalle-reclamacion-actuacion-cc/verdetalle-reclamacion-actuacion-cc.component';
import { VerdetalleReclamacionAsegCcComponent } from './components/verdetalle-reclamacion-aseg-cc/verdetalle-reclamacion-aseg-cc.component';
import { VerdetalleTramiteCcComponent } from './components/verdetalle-tramite-cc/verdetalle-tramite-cc.component';
import { VerdetalleeditTramiteCntrvContrcComponent } from './components/verdetalleedit-tramite-cntrv-contrc/verdetalleedit-tramite-cntrv-contrc.component';

const routes: Routes = [
  {
    path: '',
    component: GestionarTramiteControvrContractComponent
  },
  {
    path: 'registrarControversiaContractual',
    component: FormRegistrarControversiaContractuaComponent
  },
  {
    path: 'verDetalleEditarControversia/:id',
    component: VerDetalleditarCntrvContrcComponent
  },
  {
    path: 'actualizarTramiteControversia/:id',
    component: ActualizarTramiteContvrContrcComponent
  },
  {
    path: 'registrarNuevaActuacionTramite/:idControversia/:idActuacion',
    component: RegistrarNuevaActuacionTramiteComponent
  },
  {
    path: 'verDetalleEditarTramite/:idControversia/:id',
    component: VerdetalleeditTramiteCntrvContrcComponent
  },
  {
    path: 'verDetalleActuacionTramite/:idControversia/:id',
    component: VerDetalleActuacionContrContrctComponent
  },
  {
    path: 'verDetalleControversia/:id',
    component: VerdetalleTramiteCcComponent
  },
  {
    path: 'registrarReclamacionAseguradora/:idControversia/:id',
    component: RegistarReclamacionAseguradoraCcComponent
  },
  {
    path: 'verDetalleEditarReclamacionAseguradora/:idControversia/:id',
    component: VerDetalleeditarReclamacionComponent
  },
  {
    path: 'verDetalleReclamacionAseguradora/:idControversia/:id',
    component: VerdetalleReclamacionAsegCcComponent
  },
  {
    path: 'actualizarReclamoAseguradora/:idControversia/:idReclamacion',
    component: ActualizarReclamacionAsegCcComponent
  },
  {
    path: 'registrarNuevaActuacionReclamacion/:idControversia/:idReclamacion/:id',
    component: RegistrarNuevaActuacionReclamacionComponent
  },
  {
    path: 'verDetalleEditarActuacionReclamacion/:idControversia/:idReclamacion/:id',
    component: VerDetalleeditarActuacionReclmComponent
  },
  {
    path: 'verDetalleActuacionReclamacion/:idControversia/:idReclamacion/:id',
    component: VerdetalleReclamacionActuacionCcComponent
  },
  {
    path: 'registrarNuevaActuacionTramiteNoTai/:idControversia/:idActuacion',
    component: RegistrarNuevaActuacionTramNoTaiComponent
  },
  {
    path: 'verDetalleEditarActuacionNoTai/:idControversia/:id',
    component: VerDetalleeditarActuacionNotaiComponent
  },
  {
    path: 'verDetalleActuacionNoTai/:idControversia/:id',
    component: VerDetalleActuacionNotaiComponent
  },
  {
    path: 'registrarNuevaMesaTrabajo/:idControversia/:id',
    component: RegistrarNuevaMesatrabajoCcComponent
  },
  {
    path: 'verDetalleEditarMesaTrabajo/:idControversia/:id',
    component: VerDetalleeditarMesaDeTrabajoComponent
  },
  {
    path: 'verDetalleMesaTrabajo/:idControversia/:id',
    component: VerDetalleMesaTrabajoComponent
  },
  {
    path: 'actualizarMesaTrabajo/:idControversia/:idMesa',
    component: ActualizarMesaDeTrabajoComponent
  },
  {
    path: 'registrarNuevaMesaTrabajoAct/:idControversia/:idMesa/:id',
    component: RegistrarNuevaMesatrabajoActComponent
  },
  {
    path: 'verDetalleEditarMesaTrabajoAct/:idControversia/:idMesa/:id',
    component: VerDetalleeditarActuacionMtComponent
  },
  {
    path: 'verDetalleMesaTrabajoAct/:idControversia/:idMesa/:id',
    component: VerDetalleActuacionMtComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarTramiteControversiasContractualesRoutingModule { }
