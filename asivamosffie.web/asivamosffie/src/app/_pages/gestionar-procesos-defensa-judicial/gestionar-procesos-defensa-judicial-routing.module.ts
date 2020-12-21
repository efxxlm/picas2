import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CurrencyMaskInputMode, NgxCurrencyModule } from 'ngx-currency';
import { ActualizarProcesoDjComponent } from './components/actualizar-proceso-dj/actualizar-proceso-dj.component';
import { DetalleActuacionProcesoComponent } from './components/detalle-actuacion-proceso/detalle-actuacion-proceso.component';
import { GestionarProcesosDefensaJudicialComponent } from './components/gestionar-procesos-defensa-judicial/gestionar-procesos-defensa-judicial.component';
import { RegistrarActuacionProcesoComponent } from './components/registrar-actuacion-proceso/registrar-actuacion-proceso.component';
import { RegistroNuevoProcesoJudicialComponent } from './components/registro-nuevo-proceso-judicial/registro-nuevo-proceso-judicial.component';
import { VerDetalleEditarActuacionProcesoComponent } from './components/ver-detalle-editar-actuacion-proceso/ver-detalle-editar-actuacion-proceso.component';
import { VerDetalleEditarRegistroProcesoDjComponent } from './components/ver-detalle-editar-registro-proceso-dj/ver-detalle-editar-registro-proceso-dj.component';
export const customCurrencyMaskConfig = {
  align: "right",
  allowNegative: true,
  allowZero: true,
  decimal: ",",
  precision: 0,
  prefix: "$ ",
  suffix: "",
  thousands: ".",
  nullable: true,
  min: null,
  max: null,
  inputMode: CurrencyMaskInputMode.FINANCIAL
};

const routes: Routes = [
  {
    path: '',
    component: GestionarProcesosDefensaJudicialComponent
  },
  {
    path: 'registrarNuevoProcesoJudicial/:id',
    component: RegistroNuevoProcesoJudicialComponent
  },
  {
    path: 'verDetalleEditarProceso/:id',
    component: VerDetalleEditarRegistroProcesoDjComponent
  },
  {
    path: 'actualizarProceso/:id',
    component: ActualizarProcesoDjComponent
  },
  {
    path: 'registrarActuacionProceso',
    component: RegistrarActuacionProcesoComponent
  },
  {
    path: 'verDetalleEditarActuacionProceso/:id',
    component: VerDetalleEditarActuacionProcesoComponent
  },
  {
    path: 'verDetalleActuacion/:id',
    component: DetalleActuacionProcesoComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes),NgxCurrencyModule.forRoot(customCurrencyMaskConfig)],
  exports: [RouterModule]
})
export class GestionarProcesosDefensaJudicialRoutingModule { }