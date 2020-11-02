import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { GenerarRegistroPresupuestalComponent } from './components/generar-registro-presupuestal/generar-registro-presupuestal.component';
import { GenerarRegistroPresupuestalRoutingModule } from './generar-registro-presupuestal-routing.module';
import { TablaRegistroPresupuestalComponent } from './components/tabla-registro-presupuestal/tabla-registro-presupuestal.component';
import { GestionarDrpComponent } from './components/gestionar-drp/gestionar-drp.component';
import { VerDetalleRegistroPresupuestalComponent } from './components/ver-detalle-registro-presupuestal/ver-detalle-registro-presupuestal.component';
import { CancelarDrpComponent } from './components/cancelar-drp/cancelar-drp.component';
import { QuillModule } from 'ngx-quill';

@NgModule({
  declarations: [GenerarRegistroPresupuestalComponent, TablaRegistroPresupuestalComponent, GestionarDrpComponent, VerDetalleRegistroPresupuestalComponent, CancelarDrpComponent],
  imports: [
    CommonModule,
    GenerarRegistroPresupuestalRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    QuillModule.forRoot()
  ]
})
export class GenerarRegistroPresupuestalModule { }
