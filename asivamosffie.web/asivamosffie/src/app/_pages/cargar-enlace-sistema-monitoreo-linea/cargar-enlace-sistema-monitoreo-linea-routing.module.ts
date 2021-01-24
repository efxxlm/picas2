import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CargarEnlaceSistemaMonitoreoLineaComponent } from './components/cargar-enlace-sistema-monitoreo-linea/cargar-enlace-sistema-monitoreo-linea.component';

const routes: Routes = [
  {
    path: '',
    component: CargarEnlaceSistemaMonitoreoLineaComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CargarEnlaceSistemaMonitoreoLineaRoutingModule { }
