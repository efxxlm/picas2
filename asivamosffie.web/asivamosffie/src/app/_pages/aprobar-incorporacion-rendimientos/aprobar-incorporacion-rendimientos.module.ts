import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AprobarIncorporacionRendimientosRoutingModule } from './aprobar-incorporacion-rendimientos-routing.module';
import { AprobarIncorporacionRendimientosComponent } from './components/aprobar-incorporacion-rendimientos/aprobar-incorporacion-rendimientos.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { DialogCargarActaFirmadaAirComponent } from './components/dialog-cargar-acta-firmada-air/dialog-cargar-acta-firmada-air.component';

@NgModule({
  declarations: [AprobarIncorporacionRendimientosComponent, DialogCargarActaFirmadaAirComponent],
  imports: [
    AprobarIncorporacionRendimientosRoutingModule,
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class AprobarIncorporacionRendimientosModule { }
