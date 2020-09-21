import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-solicitud',
  templateUrl: './form-solicitud.component.html',
  styleUrls: ['./form-solicitud.component.scss']
})
export class FormSolicitudComponent implements OnInit {

  addressForm = this.fb.group({
    estadoSolicitud   : [null, Validators.required],
    observaciones     : [null, Validators.required],
    url               : null,
    tieneCompromisos  : [null, Validators.required],
    cuantosCompromisos: [null, Validators.required],
    compromisos       : this.fb.array([])
  });

  estadosArray = [
    { 
      name: 'Devuelto por comité', 
      value: 'devueltoComite' 
    }
  ];

  get compromisos() {
    return this.addressForm.get('compromisos') as FormArray;
  };

  constructor( private fb: FormBuilder ) { };

  ngOnInit(): void {
  };

}
