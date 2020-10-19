import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VisualizarAvanceObraTiempoRealRoutingModule } from './visualizar-avance-obra-tiempo-real-routing.module';
import { VisualizarAvanceObraTiempoRealComponent } from './components/visualizar-avance-obra-tiempo-real/visualizar-avance-obra-tiempo-real.component';
import { AcordionTablaListaProyectosVaotrComponent } from './components/acordion-tabla-lista-proyectos-vaotr/acordion-tabla-lista-proyectos-vaotr.component';
import { MaterialModule } from 'src/app/material/material.module';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [VisualizarAvanceObraTiempoRealComponent, AcordionTablaListaProyectosVaotrComponent],
  imports: [
    VisualizarAvanceObraTiempoRealRoutingModule,
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
  ]
})
export class VisualizarAvanceObraTiempoRealModule { }
