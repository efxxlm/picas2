import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VerificarInformeFinalProyectoRoutingModule } from './verificar-informe-final-proyecto-routing.module';
import { TablaInformeFinalComponent } from './components/tabla-informe-final/tabla-informe-final.component';
import { HomeComponent } from './components/home/home.component';


@NgModule({
  declarations: [TablaInformeFinalComponent, HomeComponent],
  imports: [
    CommonModule,
    VerificarInformeFinalProyectoRoutingModule
  ]
})
export class VerificarInformeFinalProyectoModule { }
