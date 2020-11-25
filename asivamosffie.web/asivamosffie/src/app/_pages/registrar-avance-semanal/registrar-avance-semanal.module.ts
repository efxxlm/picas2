import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RegistrarAvanceSemanalRoutingModule } from './registrar-avance-semanal-routing.module';
import { RegistrarAvanceSemanalComponent } from './components/registrar-avance-semanal/registrar-avance-semanal.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { TablaRegistrarAvanceSemanalComponent } from './components/tabla-registrar-avance-semanal/tabla-registrar-avance-semanal.component';
import { FormRegistrarSeguimientoSemanalComponent } from './components/form-registrar-seguimiento-semanal/form-registrar-seguimiento-semanal.component';
import { AvanceFisicoFinancieroComponent } from './components/avance-fisico-financiero/avance-fisico-financiero.component';
import { TablaAvanceResumenAlertasComponent } from './components/tabla-avance-resumen-alertas/tabla-avance-resumen-alertas.component';
import { TablaAvanceFisicoComponent } from './components/tabla-avance-fisico/tabla-avance-fisico.component';
import { DialogTablaAvanceResumenComponent } from './components/dialog-tabla-avance-resumen/dialog-tabla-avance-resumen.component';
import { DialogAvanceAcumuladoComponent } from './components/dialog-avance-acumulado/dialog-avance-acumulado.component';
import { GestionDeObraComponent } from './components/gestion-de-obra/gestion-de-obra.component';
import { GestionAmbientalComponent } from './components/gestion-ambiental/gestion-ambiental.component';
import { GestionCalidadComponent } from './components/gestion-calidad/gestion-calidad.component';
import { GestionSSTComponent } from './components/gestion-sst/gestion-sst.component';
import { GestionSocialComponent } from './components/gestion-social/gestion-social.component';
import { AlertasRelevantesComponent } from './components/alertas-relevantes/alertas-relevantes.component';
import { RegistrarResultadosEnsayoComponent } from './components/registrar-resultados-ensayo/registrar-resultados-ensayo.component';
import { ReporteActividadesComponent } from './components/reporte-actividades/reporte-actividades.component';
import { FormReporteActividadesRealizadasComponent } from './components/form-reporte-actividades-realizadas/form-reporte-actividades-realizadas.component';
import { RegistroFotograficoComponent } from './components/registro-fotografico/registro-fotografico.component';
import { ComiteObraComponent } from './components/comite-obra/comite-obra.component';
import { ConsultarEditarBitacoraComponent } from './components/consultar-editar-bitacora/consultar-editar-bitacora.component';
import { TablaConsultarEditarBitacoraComponent } from './components/tabla-consultar-editar-bitacora/tabla-consultar-editar-bitacora.component';
import { VerDetalleAvanceSemanalComponent } from './components/ver-detalle-avance-semanal/ver-detalle-avance-semanal.component';
import { AvanceFinancieroComponent } from './components/avance-financiero/avance-financiero.component';
import { VerDetalleMuestrasComponent } from './components/ver-detalle-muestras/ver-detalle-muestras.component';


@NgModule({
  declarations: [RegistrarAvanceSemanalComponent, TablaRegistrarAvanceSemanalComponent, FormRegistrarSeguimientoSemanalComponent, AvanceFisicoFinancieroComponent, TablaAvanceResumenAlertasComponent, TablaAvanceFisicoComponent, DialogTablaAvanceResumenComponent, DialogAvanceAcumuladoComponent, GestionDeObraComponent, GestionAmbientalComponent, GestionCalidadComponent, GestionSSTComponent, GestionSocialComponent, AlertasRelevantesComponent, RegistrarResultadosEnsayoComponent, ReporteActividadesComponent, FormReporteActividadesRealizadasComponent, RegistroFotograficoComponent, ComiteObraComponent, ConsultarEditarBitacoraComponent, TablaConsultarEditarBitacoraComponent, VerDetalleAvanceSemanalComponent, AvanceFinancieroComponent, VerDetalleMuestrasComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    QuillModule.forRoot(),
    RegistrarAvanceSemanalRoutingModule
  ]
})
export class RegistrarAvanceSemanalModule { }
