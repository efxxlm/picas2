import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { InicioComponent } from './components/inicio/inicio.component';

import { InicioRoutingModule } from './inicio-routing.module';
import { MaterialModule } from './../../material/material.module';

@NgModule({
  declarations: [
    InicioComponent,
  ],
  imports: [
    InicioRoutingModule,
    MaterialModule,
    ReactiveFormsModule
  ]
})

export class InicioModule {

}
