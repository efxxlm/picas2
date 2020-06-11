import { NgModule } from '@angular/core';

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
  ]
})

export class InicioModule {

}
