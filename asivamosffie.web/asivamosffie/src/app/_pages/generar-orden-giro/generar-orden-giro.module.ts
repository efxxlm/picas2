import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GenerarOrdenGiroRoutingModule } from './generar-orden-giro-routing.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { GenerarOrdenGiroComponent } from './components/generar-orden-giro/generar-orden-giro.component';



@NgModule({
  declarations: [GenerarOrdenGiroComponent],
  imports: [
    GenerarOrdenGiroRoutingModule,
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot(),
  ]
})
export class GenerarOrdenGiroModule { }
