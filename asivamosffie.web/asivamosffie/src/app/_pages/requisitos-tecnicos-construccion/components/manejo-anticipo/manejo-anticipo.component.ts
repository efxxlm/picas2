import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-manejo-anticipo',
  templateUrl: './manejo-anticipo.component.html',
  styleUrls: ['./manejo-anticipo.component.scss']
})
export class ManejoAnticipoComponent implements OnInit {

  formAnticipo: FormGroup;

  constructor ( private fb: FormBuilder ) {
    this.crearFormulario();
  }

  ngOnInit(): void {
  }

  crearFormulario () {
    this.formAnticipo = this.fb.group({
      requiereAnticipo: [ null ],
      planInversionAnticipo: [ null ],
      cronogramaAmortizacionAprobado: [ null ],
      urlSoporte: [ null ]
    });
  };

  guardar () {
    console.log( this.formAnticipo );
  }

};