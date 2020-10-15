import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'src/app/material/material.module';
import { QuillModule } from 'ngx-quill';
import { ReactiveFormsModule } from '@angular/forms';
import { CargarEnlaceSistemaMonitoreoLineaComponent } from './components/cargar-enlace-sistema-monitoreo-linea/cargar-enlace-sistema-monitoreo-linea.component';
import { CargarEnlaceSistemaMonitoreoLineaRoutingModule } from './cargar-enlace-sistema-monitoreo-linea-routing.module';
import { AcordionTablaListaProyectosCesmlComponent } from './components/acordion-tabla-lista-proyectos-cesml/acordion-tabla-lista-proyectos-cesml.component';



@NgModule({
  declarations: [CargarEnlaceSistemaMonitoreoLineaComponent, AcordionTablaListaProyectosCesmlComponent],
  imports: [
    CommonModule,
    MaterialModule,
    QuillModule.forRoot(),
    ReactiveFormsModule,
    CargarEnlaceSistemaMonitoreoLineaRoutingModule
  ]
})
export class CargarEnlaceSistemaMonitoreoLineaModule { }
