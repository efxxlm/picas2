import { Component } from '@angular/core';
import { FormBuilder, Validators, FormArray, ControlValueAccessor, FormGroup, FormControl } from '@angular/forms';
import { CofinanciacionService, CofinanciacionAportante } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';

@Component({
  selector: 'app-registrar-acuerdo',
  templateUrl: './registrar-acuerdo.component.html',
  styleUrls: ['./registrar-acuerdo.component.scss']
})
export class RegistrarAcuerdoComponent {
  addressForm = this.fb.group({
    vigenciaEstado: ['', Validators.required],
    numAportes: ['', [Validators.required, Validators.maxLength(2), Validators.min(1), Validators.max(99)]],
    aportantes: this.fb.array([
      // [{
      //     tipo: ['', Validators.required],
      //     nombre: ['', Validators.required],
      //     cauntosDocumentos: ['', [Validators.required, Validators.maxLength(2), Validators.min(1), Validators.max(99)]],
      //   }],
      // [{
      //   tipo: ['', Validators.required],
      //   nombre: ['', Validators.required],
      //   cauntosDocumentos: ['', [Validators.required, Validators.maxLength(2), Validators.min(1), Validators.max(99)]],
      // }]
    ])
  });

  vigenciaEstados = [2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024];
  selectTiposAportante = [
    { name: 'primero', value: 1 }, { name: 'segundo', value: 2 }, { name: 'tercero', value: 3 }
  ];
  nombresAportante = [
    { name: 'fundacion 1', value: 1 }, { name: 'fundacion 2', value: 2 }, { name: 'fundacion 3', value: 3 }
  ];


  constructor(private fb: FormBuilder,
              private cofinanciacionService: CofinanciacionService) {
    console.log(this.addressForm.value);
    this.addressForm.valueChanges
    .subscribe(value => {
      console.log(value);
    });

  }

  createAportante(): FormGroup {
    return this.fb.group({
      tipo: ['', Validators.required],
      nombre: ['', Validators.required],
      cauntosDocumentos: ['', [Validators.required, Validators.maxLength(2), Validators.min(1), Validators.max(99)]],
    });
  }

  CambioNumeroAportantes(){
    this.aportantes.push( this.createAportante() );
  }

  get aportantes() {
    return this.addressForm.get('aportantes') as FormArray;
  }

  // agregarAportante(e: number) {  (change)="agregarAportante(e)"
  //   console.log(e);
  //   this.aportantes.push(
  //     this.fb.group({
  //       tipo: ['', Validators.required],
  //       nombre: ['', Validators.required],
  //       cauntosDocumentos: ['', [Validators.required, Validators.maxLength(2), Validators.min(1), Validators.max(99)]],
  //     })
  //   );
  // }

  onSubmit() {
    if (this.addressForm.valid) {
      console.log(this.addressForm.value);
    }
  }
}
