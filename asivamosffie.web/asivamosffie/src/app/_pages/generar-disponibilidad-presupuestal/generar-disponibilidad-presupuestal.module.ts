import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { QuillModule } from 'ngx-quill';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { ReactiveFormsModule } from '@angular/forms';

import { GenerarDisponibilidadPresupuestalRoutingModule } from './generar-disponibilidad-presupuestal-routing.module';
import { MenuGenerarDisponibilidadComponent } from './components/menu-generar-disponibilidad/menu-generar-disponibilidad.component';
import { TablaConValidacionPresupuestalComponent } from './components/tabla-con-validacion-presupuestal/tabla-con-validacion-presupuestal.component';
import { GestionarDdpComponent } from './components/gestionar-ddp/gestionar-ddp.component';
import { DevolverPorValidacionComponent } from './components/devolver-por-validacion/devolver-por-validacion.component';
import { TablaConDisponibilidadPresupuestalComponent } from './components/tabla-con-disponibilidad-presupuestal/tabla-con-disponibilidad-presupuestal.component';
import { DetalleConValidacionPresupuestalComponent } from './components/detalle-con-validacion-presupuestal/detalle-con-validacion-presupuestal.component';
import { TablaDevueltaPorCoordinacionFinancieraComponent } from './components/tabla-devuelta-por-coordinacion-financiera/tabla-devuelta-por-coordinacion-financiera.component';
import { DevueltaPorCoordinacionFinancieraComponent } from './components/devuelta-por-coordinacion-financiera/devuelta-por-coordinacion-financiera.component';
import { TablaProyectosAsociadosComponent } from './components/tabla-proyectos-asociados/tabla-proyectos-asociados.component';
import { CancelarDdpComponent } from './components/cancelar-ddp/cancelar-ddp.component';
import { TablaConDisponibilidadCanceladaComponent } from './components/tabla-con-disponibilidad-cancelada/tabla-con-disponibilidad-cancelada.component';
import { DetalleConDisponibilidadCanceladaComponent } from './components/detalle-con-disponibilidad-cancelada/detalle-con-disponibilidad-cancelada.component';
import { TablaProyectosAsociadosCanceladaComponent } from './components/tabla-proyectos-asociados-cancelada/tabla-proyectos-asociados-cancelada.component';

@NgModule({
  declarations: [MenuGenerarDisponibilidadComponent, TablaConValidacionPresupuestalComponent, GestionarDdpComponent, DevolverPorValidacionComponent, TablaConDisponibilidadPresupuestalComponent, DetalleConValidacionPresupuestalComponent, TablaDevueltaPorCoordinacionFinancieraComponent, DevueltaPorCoordinacionFinancieraComponent, TablaProyectosAsociadosComponent, CancelarDdpComponent, TablaConDisponibilidadCanceladaComponent, DetalleConDisponibilidadCanceladaComponent, TablaProyectosAsociadosCanceladaComponent],
  imports: [
    CommonModule,
    GenerarDisponibilidadPresupuestalRoutingModule,
    MaterialModule,
    QuillModule.forRoot(),
    ReactiveFormsModule,
    CurrencyMaskModule
  ]
})
export class GenerarDisponibilidadPresupuestalModule { }
