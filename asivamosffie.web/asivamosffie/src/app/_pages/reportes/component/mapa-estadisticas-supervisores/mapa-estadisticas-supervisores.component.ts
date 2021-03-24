import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-mapa-estadisticas-supervisores',
  templateUrl: './mapa-estadisticas-supervisores.component.html',
  styleUrls: ['./mapa-estadisticas-supervisores.component.scss']
})
export class MapaEstadisticasSupervisoresComponent implements OnInit {
  addressForm: FormGroup = this.fb.group({
    vigencia: [null],
    nombreSupervisor: [null],
    nombreApoyoSupervisor: [null]
  });
  vigenciaArray = [
  ];
  nombreSupervisorArray = [
  ];
  nombreApoyoSupervisorArray = [
  ];
  gFiltro = false;
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }
  onSubmit(){

  }

}
