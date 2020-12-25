import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from './../../material/material.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VerificarAvanceSemanalRoutingModule } from './verificar-avance-semanal-routing.module';
import { VerificarAvanceSemanalComponent } from './components/verificar-avance-semanal/verificar-avance-semanal.component';
import { TablaVerificarAvanceSemanalComponent } from './components/tabla-verificar-avance-semanal/tabla-verificar-avance-semanal.component';
import { QuillModule } from 'ngx-quill';
import { FormVerificarSeguimientoSemanalComponent } from './components/form-verificar-seguimiento-semanal/form-verificar-seguimiento-semanal.component';
import { VerificarAvanceFisicoComponent } from './components/verificar-avance-fisico/verificar-avance-fisico.component';
import { TablaAvanceResumenAlertasComponent } from './components/tabla-avance-resumen-alertas/tabla-avance-resumen-alertas.component';
import { DialogAvanceResumenAlertasComponent } from './components/dialog-avance-resumen-alertas/dialog-avance-resumen-alertas.component';
import { TablaDisponibilidadMaterialComponent } from './components/tabla-disponibilidad-material/tabla-disponibilidad-material.component';
import { TablaAvanceFisicoComponent } from './components/tabla-avance-fisico/tabla-avance-fisico.component';
import { DialogAvanceAcumuladoComponent } from './components/dialog-avance-acumulado/dialog-avance-acumulado.component';
import { AvanceFinancieroComponent } from './components/avance-financiero/avance-financiero.component';
import { TablaAvanceFinancieroComponent } from './components/tabla-avance-financiero/tabla-avance-financiero.component';
import { GestionDeObraComponent } from './components/gestion-de-obra/gestion-de-obra.component';
import { GestionAmbientalComponent } from './components/gestion-ambiental/gestion-ambiental.component';
import { GestionCalidadComponent } from './components/gestion-calidad/gestion-calidad.component';
import { GestionSstComponent } from './components/gestion-sst/gestion-sst.component';
import { GestionSocialComponent } from './components/gestion-social/gestion-social.component';
import { AlertasRelevantesComponent } from './components/alertas-relevantes/alertas-relevantes.component';
import { ManejoMaterialInsumoComponent } from './components/manejo-material-insumo/manejo-material-insumo.component';
import { ManejoResiduosConstruccionComponent } from './components/manejo-residuos-construccion/manejo-residuos-construccion.component';
import { ManejoResiduosPeligrososComponent } from './components/manejo-residuos-peligrosos/manejo-residuos-peligrosos.component';
import { OtrosManejosComponent } from './components/otros-manejos/otros-manejos.component';
import { ReporteActividadesComponent } from './components/reporte-actividades/reporte-actividades.component';
import { FormReporteActividadesRealizadasComponent } from './components/form-reporte-actividades-realizadas/form-reporte-actividades-realizadas.component';
import { RegistroFotograficoComponent } from './components/registro-fotografico/registro-fotografico.component';
import { ComiteObraComponent } from './components/comite-obra/comite-obra.component';


@NgModule({
  declarations: [VerificarAvanceSemanalComponent, TablaVerificarAvanceSemanalComponent, FormVerificarSeguimientoSemanalComponent, VerificarAvanceFisicoComponent, TablaAvanceResumenAlertasComponent, DialogAvanceResumenAlertasComponent, TablaDisponibilidadMaterialComponent, TablaAvanceFisicoComponent, DialogAvanceAcumuladoComponent, AvanceFinancieroComponent, TablaAvanceFinancieroComponent, GestionDeObraComponent, GestionAmbientalComponent, GestionCalidadComponent, GestionSstComponent, GestionSocialComponent, AlertasRelevantesComponent, ManejoMaterialInsumoComponent, ManejoResiduosConstruccionComponent, ManejoResiduosPeligrososComponent, OtrosManejosComponent, ReporteActividadesComponent, FormReporteActividadesRealizadasComponent, RegistroFotograficoComponent, ComiteObraComponent],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    QuillModule.forRoot(),
    VerificarAvanceSemanalRoutingModule
  ]
})
export class VerificarAvanceSemanalModule { }
