import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActualizarTramiteContvrContrcComponent } from './components/actualizar-tramite-contvr-contrc/actualizar-tramite-contvr-contrc.component';
import { FormRegistrarControversiaContractuaComponent } from './components/form-registrar-controversia-contractua/form-registrar-controversia-contractua.component';
import { GestionarTramiteControvrContractComponent } from './components/gestionar-tramite-controvr-contract/gestionar-tramite-controvr-contract.component';
import { RegistarReclamacionAseguradoraCcComponent } from './components/registar-reclamacion-aseguradora-cc/registar-reclamacion-aseguradora-cc.component';
import { RegistrarNuevaActuacionTramiteComponent } from './components/registrar-nueva-actuacion-tramite/registrar-nueva-actuacion-tramite.component';
import { VerDetalleActuacionContrContrctComponent } from './components/ver-detalle-actuacion-contr-contrct/ver-detalle-actuacion-contr-contrct.component';
import { VerDetalleditarCntrvContrcComponent } from './components/ver-detalleditar-cntrv-contrc/ver-detalleditar-cntrv-contrc.component';
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
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarTramiteControversiasContractualesRoutingModule { }
