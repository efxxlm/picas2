import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CrearProyectoTecnicoRoutingModule } from './crear-proyecto-tecnico-routing.module';
import { CrearProyectoTenicoComponent } from './components/crear-proyecto-tenico/crear-proyecto-tenico.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { FormularioProyectosComponent } from './components/formulario-proyectos/formulario-proyectos.component';
import { TablaProyectosTecnicoComponent } from './components/tabla-proyectos-tecnico/tabla-proyectos-tecnico.component';
import { MaterialModule } from 'src/app/material/material.module';
import { OchoDecimalesDirective } from '../../shared/directives/ocho-decimales/ocho-decimales.directive';

import { CurrencyMaskModule } from 'ng2-currency-mask';

@NgModule({
  declarations: [
      CrearProyectoTenicoComponent, 
      FormularioProyectosComponent, 
      TablaProyectosTecnicoComponent,
      OchoDecimalesDirective
    ],
  imports: [
    CommonModule,
    CrearProyectoTecnicoRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    CurrencyMaskModule,
    
  ]
})
export class CrearProyectoTecnicoModule { }
