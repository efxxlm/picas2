import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { GestionarActaInicioFdosConstrComponent } from './components/gestionar-acta-inicio-fdos-constr/gestionar-acta-inicio-fdos-constr.component';
import { GestionarActaInicioFdosConstrRoutingModule } from './gestionar-acta-inicio-fdos-constr-routing.module';
import { TablaGeneralActaFdosConstComponent } from './components/tabla-general-acta-fdos-const/tabla-general-acta-fdos-const.component';
import { FormGenerarActaInicioConstTecnicoComponent } from './components/form-generar-acta-inicio-const-tecnico/form-generar-acta-inicio-const-tecnico.component';
import { QuillModule } from 'ngx-quill';
import { DialogCargarActaSuscritaConstComponent } from './components/dialog-cargar-acta-suscrita-const/dialog-cargar-acta-suscrita-const.component';
import { VerDetalleTecnicoFdosConstrComponent } from './components/ver-detalle-tecnico-fdos-constr/ver-detalle-tecnico-fdos-constr.component';

@NgModule({
  declarations: [GestionarActaInicioFdosConstrComponent, TablaGeneralActaFdosConstComponent, FormGenerarActaInicioConstTecnicoComponent, DialogCargarActaSuscritaConstComponent, VerDetalleTecnicoFdosConstrComponent],
  imports: [
    CommonModule,
    GestionarActaInicioFdosConstrRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class GestionarActaInicioFdosConstrModule { }
