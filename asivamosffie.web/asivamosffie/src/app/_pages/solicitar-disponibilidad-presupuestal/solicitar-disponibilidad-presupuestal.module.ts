import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'src/app/material/material.module';
import { ReactiveFormsModule, FormsModule  } from '@angular/forms';

import { QuillModule } from 'ngx-quill';

import { SolicitarDisponibilidadPresupuestalRoutingModule } from './solicitar-disponibilidad-presupuestal-routing.module';
import { TituloComponent } from './components/titulo/titulo.component';
import { TablaCrearSolicitudTradicionalComponent } from './components/tabla-crear-solicitud-tradicional/tabla-crear-solicitud-tradicional.component';
import { RegistrarInformacionAdicionalComponent } from './components/registrar-informacion-adicional/registrar-informacion-adicional.component';
import { CrearSolicitudEspecialComponent } from './components/crear-solicitud-especial/crear-solicitud-especial.component';
import { NuevaSolicitudEspecialComponent } from './components/nueva-solicitud-especial/nueva-solicitud-especial.component';
import { TablaCrearSolicitudEspecialComponent } from './components/tabla-crear-solicitud-especial/tabla-crear-solicitud-especial.component';
import { CrearDisponibilidadPresupuestalAdministrativoComponent } from './components/crear-disponibilidad-presupuestal-administrativo/crear-administrativo.component';
import { CrearSolicitudDeDisponibilidadPresupuestalComponent } from './components/crear-disponibilidad-presupuestal/crear-disponibilidad-presupuestal.component';
import { TablaCrearSolicitudadministrativaComponent } from './components/tabla-crear-solicitud-administrativa/tabla-crear-solicitud-administrativa.component';
import { DetalleDisponibilidadPresupuestalComponent } from './components/detalle-disponibilidad-presupuestal/detalle-disponibilidad-presupuestal.component';
import { VerDetalleDdpEspecialComponent } from './components/ver-detalle-ddp-especial/ver-detalle-ddp-especial.component';
import { VerDetalleDdpAdministrativoComponent } from './components/ver-detalle-ddp-administrativo/ver-detalle-ddp-administrativo.component'
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSortModule } from '@angular/material/sort';


@NgModule({
  declarations: [
    TituloComponent,
    TablaCrearSolicitudTradicionalComponent,
    RegistrarInformacionAdicionalComponent,
    CrearSolicitudEspecialComponent,
    NuevaSolicitudEspecialComponent,
    TablaCrearSolicitudEspecialComponent,
    CrearSolicitudDeDisponibilidadPresupuestalComponent,
    CrearDisponibilidadPresupuestalAdministrativoComponent,
    TablaCrearSolicitudadministrativaComponent,
    DetalleDisponibilidadPresupuestalComponent,
    VerDetalleDdpEspecialComponent,
    VerDetalleDdpAdministrativoComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    SolicitarDisponibilidadPresupuestalRoutingModule,
    CurrencyMaskModule,
    QuillModule.forRoot(),
    MatAutocompleteModule,
    MatSortModule
  ]
})
export class SolicitarDisponibilidadPresupuestalModule { }
