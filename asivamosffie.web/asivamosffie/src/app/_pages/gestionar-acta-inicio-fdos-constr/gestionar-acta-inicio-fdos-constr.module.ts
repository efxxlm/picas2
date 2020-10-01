import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { GestionarActaInicioFdosConstrComponent } from './components/gestionar-acta-inicio-fdos-constr/gestionar-acta-inicio-fdos-constr.component';
import { GestionarActaInicioFdosConstrRoutingModule } from './gestionar-acta-inicio-fdos-constr-routing.module';
import { TablaGeneralActaFdosConstComponent } from './components/tabla-general-acta-fdos-const/tabla-general-acta-fdos-const.component';

@NgModule({
  declarations: [GestionarActaInicioFdosConstrComponent, TablaGeneralActaFdosConstComponent],
  imports: [
    CommonModule,
    GestionarActaInicioFdosConstrRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class GestionarActaInicioFdosConstrModule { }
