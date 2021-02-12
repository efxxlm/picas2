import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-lista-chequeo-rapg',
  templateUrl: './lista-chequeo-rapg.component.html',
  styleUrls: ['./lista-chequeo-rapg.component.scss']
})
export class ListaChequeoRapgComponent implements OnInit {
  addressForm = this.fb.group({
    cumpleAsegurado: [null, Validators.required],
    cumpleBeneficiario: [null, Validators.required],
    cumpleAfianzado: [null, Validators.required],
    reciboDePago: [null, Validators.required],
    condicionesGenerales: [null, Validators.required]
  });
  estaEditando = false;
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }
  onSubmit() {
    console.log(this.addressForm.value);
    this.estaEditando = true;
  }
}
