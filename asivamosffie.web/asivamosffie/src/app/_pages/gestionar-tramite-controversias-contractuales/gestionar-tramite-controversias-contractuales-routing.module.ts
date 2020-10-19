import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActualizarTramiteContvrContrcComponent } from './components/actualizar-tramite-contvr-contrc/actualizar-tramite-contvr-contrc.component';
import { FormRegistrarControversiaContractuaComponent } from './components/form-registrar-controversia-contractua/form-registrar-controversia-contractua.component';
import { GestionarTramiteControvrContractComponent } from './components/gestionar-tramite-controvr-contract/gestionar-tramite-controvr-contract.component';
import { RegistrarNuevaActuacionTramiteComponent } from './components/registrar-nueva-actuacion-tramite/registrar-nueva-actuacion-tramite.component';
import { VerDetalleditarCntrvContrcComponent } from './components/ver-detalleditar-cntrv-contrc/ver-detalleditar-cntrv-contrc.component';

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
    path: 'registrarNuevaActuacionTramite',
    component: RegistrarNuevaActuacionTramiteComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarTramiteControversiasContractualesRoutingModule { }
