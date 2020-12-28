import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { VisualizarAvanceObraTiempoRealComponent } from './components/visualizar-avance-obra-tiempo-real/visualizar-avance-obra-tiempo-real.component';

const routes: Routes = [
  {
    path: '',
    component: VisualizarAvanceObraTiempoRealComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VisualizarAvanceObraTiempoRealRoutingModule { }
