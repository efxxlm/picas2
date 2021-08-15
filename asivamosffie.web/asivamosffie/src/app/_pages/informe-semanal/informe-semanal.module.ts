import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InformeSemanalRoutingModule } from './informe-semanal-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from 'src/app/material/material.module';
import { InformeSemanalComponent } from './components/informe-semanal/informe-semanal.component';


@NgModule({
  declarations: [InformeSemanalComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    InformeSemanalRoutingModule
  ]
})
export class InformeSemanalModule { }
