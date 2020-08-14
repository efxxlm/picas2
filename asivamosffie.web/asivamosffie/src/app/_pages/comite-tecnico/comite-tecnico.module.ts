import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ComiteTecnicoRoutingModule } from './comite-tecnico-routing.module';
import { ComiteTecnicoComponent } from './components/comite-tecnico/comite-tecnico.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { MaterialModule } from './../../material/material.module';
import { CrearOrdenDelDiaComponent } from './components/crear-orden-del-dia/crear-orden-del-dia.component';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatCardModule } from '@angular/material/card';
import { TablaSolicitudesContractualesComponent } from './tabla-solicitudes-contractuales/tabla-solicitudes-contractuales.component';
import { TablaOrdenDelDiaComponent } from './components/tabla-orden-del-dia/tabla-orden-del-dia.component';
import { TablaSesionComiteTecnicoComponent } from './components/tabla-sesion-comite-tecnico/tabla-sesion-comite-tecnico.component';
import { TablaGestionActasComponent } from './components/tabla-gestion-actas/tabla-gestion-actas.component';

@NgModule({
  declarations: [ComiteTecnicoComponent, CrearOrdenDelDiaComponent, TablaSolicitudesContractualesComponent, TablaOrdenDelDiaComponent, TablaSesionComiteTecnicoComponent, TablaGestionActasComponent],
  imports: [
    CommonModule,
    ComiteTecnicoRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatRadioModule,
    MatCardModule
  ]
})
export class ComiteTecnicoModule { }
