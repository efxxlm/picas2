import { VerDetalleAvanceSemanalComponent } from './components/ver-detalle-avance-semanal/ver-detalle-avance-semanal.component';
import { TablaConsultarBitacoraComponent } from './components/tabla-consultar-bitacora/tabla-consultar-bitacora.component';
import { ComiteObraComponent } from './components/comite-obra/comite-obra.component';
import { RegistroFotograficoComponent } from './components/registro-fotografico/registro-fotografico.component';
import { FormReporteActividadesRealizadasComponent } from './components/form-reporte-actividades-realizadas/form-reporte-actividades-realizadas.component';
import { ReporteActividadesComponent } from './components/reporte-actividades/reporte-actividades.component';
import { OtrosManejosComponent } from './components/otros-manejos/otros-manejos.component';
import { ManejoResiduosPeligrososComponent } from './components/manejo-residuos-peligrosos/manejo-residuos-peligrosos.component';
import { ManejoResiduosConstruccionComponent } from './components/manejo-residuos-construccion/manejo-residuos-construccion.component';
import { ManejoMaterialInsumoComponent } from './components/manejo-material-insumo/manejo-material-insumo.component';
import { AlertasRelevantesComponent } from './components/alertas-relevantes/alertas-relevantes.component';
import { GestionSocialComponent } from './components/gestion-social/gestion-social.component';
import { GestionSstComponent } from './components/gestion-sst/gestion-sst.component';
import { GestionCalidadComponent } from './components/gestion-calidad/gestion-calidad.component';
import { GestionAmbientalComponent } from './components/gestion-ambiental/gestion-ambiental.component';
import { TablaAvanceFinancieroComponent } from './components/tabla-avance-financiero/tabla-avance-financiero.component';
import { GestionDeObraComponent } from './components/gestion-de-obra/gestion-de-obra.component';
import { AvanceFinancieroComponent } from './components/avance-financiero/avance-financiero.component';
import { DialogAvanceResumenAlertasComponent } from './components/dialog-avance-resumen-alertas/dialog-avance-resumen-alertas.component';
import { DialogAvanceAcumuladoComponent } from './components/dialog-avance-acumulado/dialog-avance-acumulado.component';
import { TablaDisponibilidadMaterialComponent } from './components/tabla-disponibilidad-material/tabla-disponibilidad-material.component';
import { TablaAvanceResumenAlertasComponent } from './components/tabla-avance-resumen-alertas/tabla-avance-resumen-alertas.component';
import { TablaAvanceFisicoComponent } from './components/tabla-avance-fisico/tabla-avance-fisico.component';
import { VerificarAvanceFisicoComponent } from './components/verificar-avance-fisico/verificar-avance-fisico.component';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from './../../material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ValidarAvanceSemanalRoutingModule } from './validar-avance-semanal-routing.module';
import { ValidarAvanceSemanalComponent } from './components/validar-avance-semanal/validar-avance-semanal.component';
import { TablaValidarAvanceSemanalComponent } from './components/tabla-validar-avance-semanal/tabla-validar-avance-semanal.component';
import { FormValidarSeguimientoSemanalComponent } from './components/form-validar-seguimiento-semanal/form-validar-seguimiento-semanal.component';
import { ConsultarBitacoraComponent } from './components/consultar-bitacora/consultar-bitacora.component';
import { VerDetalleMuestrasComponent } from './components/ver-detalle-muestras/ver-detalle-muestras.component';


@NgModule({
  declarations: [
    ValidarAvanceSemanalComponent,
    TablaValidarAvanceSemanalComponent,
    FormValidarSeguimientoSemanalComponent,
    ConsultarBitacoraComponent,
    VerDetalleMuestrasComponent,
    VerificarAvanceFisicoComponent,
    TablaAvanceFisicoComponent,
    TablaAvanceResumenAlertasComponent,
    TablaDisponibilidadMaterialComponent,
    DialogAvanceAcumuladoComponent,
    DialogAvanceResumenAlertasComponent,
    AvanceFinancieroComponent,
    TablaAvanceFinancieroComponent,
    GestionDeObraComponent,
    GestionAmbientalComponent,
    GestionCalidadComponent,
    GestionSstComponent,
    GestionSocialComponent,
    AlertasRelevantesComponent,
    ManejoMaterialInsumoComponent,
    ManejoResiduosConstruccionComponent,
    ManejoResiduosPeligrososComponent,
    OtrosManejosComponent,
    ReporteActividadesComponent,
    FormReporteActividadesRealizadasComponent,
    RegistroFotograficoComponent,
    ComiteObraComponent,
    TablaConsultarBitacoraComponent,
    VerDetalleAvanceSemanalComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    QuillModule.forRoot(),
    ValidarAvanceSemanalRoutingModule
  ]
})
export class ValidarAvanceSemanalModule { }
