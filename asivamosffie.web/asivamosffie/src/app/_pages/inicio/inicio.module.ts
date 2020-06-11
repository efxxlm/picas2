import { NgModule } from '@angular/core';

import { InicioComponent } from './components/inicio/inicio.component';

import { InicioRoutingModule } from './inicio-routing.module';

@NgModule({
  declarations: [
    InicioComponent,
  ],
  imports: [
    InicioRoutingModule
  ]
})

export class InicioModule {

}
