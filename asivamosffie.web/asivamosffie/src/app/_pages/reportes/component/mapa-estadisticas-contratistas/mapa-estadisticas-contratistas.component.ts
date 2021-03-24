import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-mapa-estadisticas-contratistas',
  templateUrl: './mapa-estadisticas-contratistas.component.html',
  styleUrls: ['./mapa-estadisticas-contratistas.component.scss']
})
export class MapaEstadisticasContratistasComponent implements OnInit {
  addressForm: FormGroup = this.fb.group({
    vigencia: [null],
    tipoContratista: [null],
    nombreContratista: [null]
  });
  vigenciaArray = [
  ];
  tipoContratistaArray = [
  ];
  nombreContratistaArray = [
  ];
  gFiltro = false;
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }
  onSubmit(){

  }
}
