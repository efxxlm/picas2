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
import { RegistrarNuevaMesatrabajoCcComponent } from './components/registrar-nueva-mesatrabajo-cc/registrar-nueva-mesatrabajo-cc.component';
import { VerDetalleActuacionContrContrctComponent } from './components/ver-detalle-actuacion-contr-contrct/ver-detalle-actuacion-contr-contrct.component';
import { VerDetalleActuacionNotaiComponent } from './components/ver-detalle-actuacion-notai/ver-detalle-actuacion-notai.component';
import { VerDetalleditarCntrvContrcComponent } from './components/ver-detalleditar-cntrv-contrc/ver-detalleditar-cntrv-contrc.component';
import { VerDetalleeditarActuacionNotaiComponent } from './components/ver-detalleeditar-actuacion-notai/ver-detalleeditar-actuacion-notai.component';
import { VerDetalleeditarActuacionReclmComponent } from './components/ver-detalleeditar-actuacion-reclm/ver-detalleeditar-actuacion-reclm.component';
import { VerDetalleeditarMesaDeTrabajoComponent } from './components/ver-detalleeditar-mesa-de-trabajo/ver-detalleeditar-mesa-de-trabajo.component';
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
    path: 'actualizarTramiteControversia',
    component: ActualizarTramiteContvrContrcComponent
  },
  {
    path: 'registrarNuevaActuacionTramite',
    component: RegistrarNuevaActuacionTramiteComponent
  },
  {
    path: 'verDetalleEditarTramite/:id',
    component: VerdetalleeditTramiteCntrvContrcComponent
  },
  {
    path: 'verDetalleActuacionTramite/:id',
    component: VerDetalleActuacionContrContrctComponent
  },
  {
    path: 'verDetalleControversia/:id',
    component: VerdetalleTramiteCcComponent
  },
  {
    path: 'registrarReclamacionAseguradora/:id',
    component: RegistarReclamacionAseguradoraCcComponent
  },
  {
    path: 'verDetalleReclamacionAseguradora/:id',
    component: VerdetalleReclamacionAsegCcComponent
  },
  {
    path: 'actualizarReclamoAseguradora',
    component: ActualizarReclamacionAsegCcComponent
  },
  {
    path: 'registrarNuevaActuacionReclamacion',
    component: RegistrarNuevaActuacionReclamacionComponent
  },
  {
    path: 'verDetalleEditarActuacionReclamacion/:id',
    component: VerDetalleeditarActuacionReclmComponent
  },
  {
    path: 'verDetalleActuacionReclamacion/:id',
    component: VerdetalleReclamacionActuacionCcComponent
  },
  {
    path: 'registrarNuevaActuacionTramiteNoTai',
    component: RegistrarNuevaActuacionTramNoTaiComponent
  },
  {
    path: 'verDetalleEditarActuacionNoTai/:id',
    component: VerDetalleeditarActuacionNotaiComponent
  },
  {
    path: 'verDetalleActuacionNoTai/:id',
    component: VerDetalleActuacionNotaiComponent
  },
  {
    path: 'registrarNuevaMesaTrabajo/:id',
    component: RegistrarNuevaMesatrabajoCcComponent
  },
  {
    path: 'verDetalleEditarMesaTrabajo/:id',
    component: VerDetalleeditarMesaDeTrabajoComponent
  },
  {
    path: 'actualizarMesaTrabajo',
    component: ActualizarMesaDeTrabajoComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarTramiteControversiasContractualesRoutingModule { }
