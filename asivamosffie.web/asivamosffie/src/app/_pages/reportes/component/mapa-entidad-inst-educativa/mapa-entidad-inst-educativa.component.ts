import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-mapa-entidad-inst-educativa',
  templateUrl: './mapa-entidad-inst-educativa.component.html',
  styleUrls: ['./mapa-entidad-inst-educativa.component.scss']
})
export class MapaEntidadInstEducativaComponent implements OnInit {
  addressForm: FormGroup = this.fb.group({
    vigencia: [null],
    departamento: [null],
    municipio: [null],
    institucionEducativa: [null]
  });
  vigenciaArray = [
  ];
  departamentoArray = [
  ];
  municipioArray = [
  ];
  institucionEduArray=[

  ];
  gFiltro = false;
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }
  onSubmit(){

  }
}
